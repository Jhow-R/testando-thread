using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestandoThread
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Esta é a thread principal do programa");
        }

        private void ThreadTarefa()
        {
            int step, novoValor = 0;
            Random rnd = new Random();

            while (novoValor < pgbThreads.Maximum)
            {
                if (pgbThreads.Value == 0)
                    step = pgbThreads.Step + rnd.Next(0, 15);
                else
                    step = pgbThreads.Step + rnd.Next(-25, 15);

                novoValor = pgbThreads.Value + step;

                if (novoValor > pgbThreads.Maximum)
                {
                    novoValor = pgbThreads.Maximum;
                    SetControlPropertyValue(pgbThreads, "value", novoValor);
                }
                else
                    SetControlPropertyValue(pgbThreads, "value", novoValor);

                Thread.Sleep(100);
            }

            MessageBox.Show("Thread concluída");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread trd = new Thread(new ThreadStart(ThreadTarefa));
            trd.IsBackground = true;
            trd.Start();
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();

                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
    }
}
