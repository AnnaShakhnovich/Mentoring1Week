using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mentoringWeek1
{
    class FileSystemVisitor
    {
        private readonly Predicate<string> predicate;
        private int level = 0;
        public FileSystemVisitor(Predicate<string> predicate)
        {
            this.predicate = predicate;
        }

        public IEnumerable<string> DirSearch(string sDir, bool isFirstTimeCalled = true)
        {
            if (isFirstTimeCalled)
            {
                Start?.Invoke(this, null);
            }
            if (Directory.Exists(sDir))
            {
                foreach (string file in Directory.GetFiles(sDir))
                {
                    var args = new FoundEventArgs();
                    args.Name = $"File {file}";
                    args.IsFiltrationPassed = predicate(file);
                    OnFileFoundReached(args);
                    if (args.IsFiltrationPassed && !args.ExcludeEntry)
                        yield return file;
                }
                foreach (string directory in Directory.GetDirectories(sDir))
                {
                    var args = new FoundEventArgs();
                    args.Name = $"Directory {directory}";
                    args.IsFiltrationPassed = predicate(directory);
                    OnDitectoryFoundReached(args);
                    if (args.IsFiltrationPassed)
                        yield return directory;
                    foreach (var dirSearch in DirSearch(directory, false))
                    {
                        yield return dirSearch;
                    }
                }
            }
            if (isFirstTimeCalled)
            {
                Finish?.Invoke(this, null);
            }
        }

        protected void OnFileFoundReached(FoundEventArgs e)
        {
            FileFound?.Invoke(this, e);
        }

        protected void OnDitectoryFoundReached(FoundEventArgs e)
        {
            DitectoryFound?.Invoke(this, e);
        }

        public event EventHandler<FoundEventArgs> FileFound;
        public event EventHandler<FoundEventArgs> DitectoryFound;
        public event EventHandler Start;
        public event EventHandler Finish;
    }

    public class FoundEventArgs : EventArgs
    {
        public string Name { get; set; }
        public bool IsFiltrationPassed { get; set; }
        public bool ExcludeEntry { get; set; }
        public bool CancelSearch { get; set; }
    }
}
