using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FadeFox.UI
{
    [
    DefaultProperty("Interval"),
    DefaultEvent("Tick"),
    ToolboxBitmap(typeof(System.Windows.Forms.Timer))
    ]
    public partial class NPTimer : Component, IDisposable
    {
        public NPTimer()
            : base()
        {
            _interval = 100;
            InitializeComponent();
        }

        public NPTimer(IContainer container)
            : this()
        {
            container.Add(this);
            InitializeComponent();
        }

        private Timer ScadularTimer;
        private TimerCanceller timerCanceller = new TimerCanceller();
        private event EventHandler tick;
        private bool _status = false;
        private object syncObj = new object();
        private int _interval;

        public event EventHandler Tick
        {
            add => tick += value;
            remove => tick -= value;
        }

        [Browsable(true), DefaultValue(100)]
        public int Interval
        {
            get { return this._interval; }
            set
            {
                this._interval = value;
                if (_status) ScadularTimer.Change(0, _interval);
            }
        }

        [DefaultValue(false)]
        public bool Enabled
        {
            get
            {
                return _status;
            }
            set
            {
                lock (syncObj)
                {
                    if(_status != value)
                    {
                        if (!DesignMode)
                        {
                            if (value) Start();
                            else Stop();
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            if (_status)
            {
                if (ScadularTimer != null)
                {
                    timerCanceller.Cancelled = true;
                    ScadularTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    _status = false;
                }
            }
        }

        public void Start()
        {
            if (!_status)
            {
                if (ScadularTimer == null)
                {
                    ScadularTimer = new System.Threading.Timer(TimerCallBack, timerCanceller, 0, Timeout.Infinite);
                }
                timerCanceller.Cancelled = false;
                ScadularTimer.Change(0, _interval);
                _status = true;
            }
        }

        private void TimerCallBack(object state)
        {
            try
            {
                var canceller = (TimerCanceller)state;
                if (canceller.Cancelled)
                {
                    return; //
                }

                tick?.Invoke(this, new EventArgs());

                if (canceller.Cancelled)
                {
                    //Dispose any resource that might have been initialized above
                    return; //
                }
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("A disposed object accessed");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("A nulled object accessed");
            }
            catch (Exception ex)
            {

            }
        }
    }

    class TimerCanceller
    {
        public bool Cancelled = false;
    }
}
