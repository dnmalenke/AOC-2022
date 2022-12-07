using AOC_2022.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AOC_2022.Pages
{
    [Route($"/{nameof(Day7)}")]
    public class Day7 : DayTemplate
    {
        protected override void Run()
        {
            _result = "";

            string curPath = "";
            Dictionary<string, int> sizes = new();
            List<string> files = new();
            foreach (var line in _input.Split('\n'))
            {
                if (line.StartsWith("$"))
                {
                    //  _result += $"\n{(sizes.ContainsKey(curPath) ? sizes[curPath] : 0)} {curPath} {line}";
                }

                if (line.StartsWith("$ cd"))
                {
                    if (line.Split(' ')[2] == "..")
                    {
                        curPath = curPath.Remove(curPath.TrimEnd('/').LastIndexOf('/'));
                        curPath += "/";

                    }
                    else
                    {
                        curPath += line.Split(' ')[2] + "/";
                    }

                    // _result += $"\n{curPath}";
                }

                if (!line.StartsWith("$"))
                {
                    // _result += $"\n{line}";

                    if (!sizes.ContainsKey(curPath))
                    {
                        sizes.Add(curPath, 0);
                    }

                    if (!line.StartsWith("dir"))
                    {

                        sizes[curPath] += int.Parse(line.Split(' ')[0]);

                        files.Add(curPath + line.Split(' ')[1]);
                    }
                }
            }

            Dictionary<string, int> sizes2 = new();

            foreach (var item in sizes)
            {
                if (!sizes2.ContainsKey(item.Key))
                {
                    sizes2.Add(item.Key, item.Value);
                }

                foreach (var i in sizes)
                {
                    if (i.Key.StartsWith(item.Key) && i.Key != item.Key)
                    {
                        sizes2[item.Key] += i.Value;
                    }
                }
            }

            int sum = 0;
            foreach (var item in sizes2)
            {
                if (item.Value <= 100000)
                {
                    sum += item.Value;
                }
            }

            _result += $"\npart 1 sum: {sum}";

            int totalSpace = 70000000;
            int spaceRequired = 30000000;

            int freeSpace = totalSpace - sizes2["//"];
            int spaceNeeded = spaceRequired - freeSpace;

            var toDelete = sizes2.Where(x => x.Value >= spaceNeeded).OrderBy(x => x.Value).FirstOrDefault();

            _result += $"\nspace needed {spaceNeeded}\ndeleting {toDelete.Key}, size: {toDelete.Value}";

        }
    }
}
