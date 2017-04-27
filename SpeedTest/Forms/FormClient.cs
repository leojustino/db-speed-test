using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeedTest.Tests;

namespace SpeedTest.Forms
{
    partial class FormClient : Form
    {
        public FormClient(ITestExecution testExecution)
        {
            InitializeComponent();

            this.testExecution = testExecution;

            testExecution.WaitingMutex += mutexName => label1.Text = $"Waiting {mutexName}";
        }

        ITestExecution testExecution;

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            testExecution.ExecuteTest();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }
    }
}
