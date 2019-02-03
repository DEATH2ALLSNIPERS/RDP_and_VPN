using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace RemoteDesktop.Net
{
    public class ProcessWatcherArgs
    {
        public ProcessWatcherArgs(int i, string b) { ID = i; Button = b; }
        public int ID { get; private set; } // readonly
        public string Button { get; private set; } // readonly
    }

    /// <summary>
    /// Logic for process monitoring.
    /// </summary>
    class ProcessWatcher//TODO: change string button to object button
    {
        private Dictionary<int, string> rdplist;
        private bool _stop { get; set; } = false;
        private readonly string processName = null;
        private bool isAddNewByClick = false;

        public delegate void ClosedProcessHandler(object sender, ProcessWatcherArgs e);
        /// <summary>
        /// Launch if the process from the list is closed.
        /// </summary>
        public event ClosedProcessHandler ClosedProcess;

        /// <summary>
        /// Initializes a new instance of the ProcessWatcher class with the process name specified as an <see cref="string"/>.
        /// </summary>
        /// <param name="processName">Process name for wotching.</param>
        public ProcessWatcher(string processName)
        {
            rdplist = new Dictionary<int, string>();
            this.processName = processName;
            FillProcess();

            Task.Run(() => {
                //Here is a new thread
                while (!_stop)
                {
                    Thread.Sleep(1000);//1s
                    Debug.WriteLine($"ProcessWatcher: Working:{rdplist.Count}");
                    try
                    {
                        if (NumberOfProcess() == Difference.Less)
                        {
                            DeleteNotUsedProcess();
                        }
                        if (!isAddNewByClick)
                        {
                            if (NumberOfProcess() == Difference.Greater)
                            {
                                FindNewProcess();//dodawanie procesów uruchomionych poza apk.
                                isAddNewByClick = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            });
        }

        ~ProcessWatcher()
        {
            _stop = true;
            rdplist.Clear();
            Debug.WriteLine("ProcessWatcher: Stop");
        }

        private void DeleteNotUsedProcess()
        {
            var ProcesList = Process.GetProcessesByName(processName).Select(x => x.Id).ToList();
            var Diffbetweenlist = rdplist.Where(kvp => !ProcesList.Contains(kvp.Key))
                              .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            for (int i = 0; i < rdplist.Count; i++)
            {
                foreach (var item in Diffbetweenlist)
                {
                    if (rdplist.ElementAt(i).Key == item.Key)
                    {
                        var val = rdplist.ElementAt(i).Value;
                        Debug.WriteLine($"ProcessWatcher: Delete (PID:{item.Key}; { val})");
                        rdplist.Remove(item.Key);

                        ClosedProcess(this, new ProcessWatcherArgs(item.Key, val));
                    }
                }
            }
        }

        private enum Difference
        {
            Greater,
            Less,
            Equal
        }

        private Difference NumberOfProcess()
        {
            if (Process.GetProcessesByName(processName).Length == rdplist.Count) return Difference.Equal;
            else if (Process.GetProcessesByName(processName).Length > rdplist.Count) return Difference.Greater;
            else return Difference.Less;
        }

        private void FillProcess()
        {
            Debug.WriteLine("ProcessWatcher: FillList");
            foreach (var item in Process.GetProcessesByName(processName))
            {
                rdplist.Add(item.Id, null);
            }            
        }

        /// <summary>
        /// Find the process PID accompanying the new opening of the application.
        /// </summary>
        /// <param name="button"></param>
        /// <returns>if it finds a process, it will return its PID; otherwise, -1.</returns>
        public int FindNewProcess(string button = null)
        {
            Task.Run(() =>
            {
                isAddNewByClick = true;
                Thread.Sleep(500);
                int pid = NewProcessID();
                if (pid > -1)
                {
                    AddProcess(pid, button);
                }
                isAddNewByClick = false;
                return pid;
            });
            return -1;
        }

        private void AddProcess(int pid, string button)
        {
            if (!rdplist.Keys.Contains(pid))//jeśli nie zawiera
            {
                Debug.WriteLine($"ProcessWatcher: Adding (PID:{pid}; {button})");
                rdplist.Add(pid, button);
            }               
        }

        private int NewProcessID()
        {
            Process proc = Process.GetProcessesByName(processName).FirstOrDefault(x => !rdplist.Any(y => y.Key == x.Id));
            if (proc != null)
            {
                return proc.Id;
            }
            return -1;
        }

    }
}
