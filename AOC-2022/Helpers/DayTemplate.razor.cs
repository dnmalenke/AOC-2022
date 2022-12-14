namespace AOC_2022.Helpers
{
    public abstract partial class DayTemplate
    {
        protected Input _input = new();
        protected string _result = "";

        private string _title = "";
        protected override void OnInitialized()
        {
            base.OnInitialized();

            string className = GetType().Name;

            _title = className.Insert(3, " ");
        }

        protected abstract void Run();
    }
}
