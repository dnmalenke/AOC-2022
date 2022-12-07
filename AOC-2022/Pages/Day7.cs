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

            /*
             *  $ cd /
                $ ls
                dir a
                14848514 b.txt
                8504156 c.dat
                dir d
                $ cd a
                $ ls
                dir e
                29116 f
                2557 g
                62596 h.lst
                $ cd e
                $ ls
                584 i
                $ cd ..
                $ cd ..
                $ cd d
                $ ls
                4060174 j
                8033020 d.log
                5626152 d.ext
                7214296 k

            */

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
                        curPath = curPath.Remove(curPath.TrimEnd('/').LastIndexOf('/') );
                        curPath += "/";

                    }
                    else
                    {
                        curPath += line.Split(' ')[2] + "/";
                    }
                   // _result += $"\n{curPath}";
                }

                //if(line.StartsWith("$ ls"))
                //{
                //    _result += "\n LS";
                //}

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

            int sss = 0;
            var x = sizes2.FirstOrDefault(x => x.Key == "//rfgvg/");
            foreach (var item in sizes2.Where(x => x.Key.Count(k => k == '/') == 3))
            {
                _result += $"\n tld {item.Key}, {item.Value}";
                sss += item.Value;
            }
          

            int sum = 0;
            foreach (var item in sizes2)
            {                
                if (item.Value <= 100000)
                {
                    _result += $"\n adding {item.Key}, {item.Value}";
                    sum += item.Value;  
                }
            }

            _result += $"\n distinct: {files.Distinct().ToList().Count}";

            _result += $"\npart 1 sum: {sum}";

            int totalSpace = 70000000;
            int spaceRequired = 30000000;

            int freeSpace =totalSpace- sizes2["//"];
            int spaceNeeded = spaceRequired - freeSpace;

            var toDelete = sizes2.Where(x => x.Value >= spaceNeeded).OrderBy(x => x.Value).FirstOrDefault();

            _result += $"\nspace needed {spaceNeeded}\ndeleting {toDelete.Key}, size: {toDelete.Value}";
            
        }
    }
}
