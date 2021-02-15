using DSoft.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFSample
{
    public class MainViewModel : ViewModel
    {
        private const string EventId = "42B65ED9-62CB-4AC7-A1B9-2A879C6BB8F6";

        private string _messageLog;

        public string MessageLog
        {
            get { return _messageLog; }
            set { _messageLog = value; NotifyPropertyChanged(nameof(MessageLog)); }
        }

        public ICommand BeginCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBus.RunPostOnSeperateTask = true;

                    MessageLog = string.Empty;

                    var startTime = DateTime.Now;

                    for (var loop = 0; loop < 1000; loop++)
                    {
                        MessageBus.Post(EventId);
                    }

                    var ednTime = DateTime.Now;

                    var diff = ednTime - startTime;

                    MessageLog += $"Total time: {diff.TotalSeconds}";

                    NotifyOnComplete(true);

                });
            }
        }

        //Total time: 3.3507754
        public MainViewModel()
        {
            MessageBus.Subscribe(EventId, () =>
            {
                MessageLog += "Tick" + Environment.NewLine;

            });
        }
    }
}
