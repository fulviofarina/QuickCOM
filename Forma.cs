using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace QuickCOM
{
    public partial class Forma : Form
    {
        private int lastIndex = 0;

        public void SetControlText(RichTextBox control, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<RichTextBox, string>(SetControlText), new object[] { control, text });
            }
            else
            {
                control.AppendText(text);
            }
        }
     

      

        public Forma()
        {
            InitializeComponent();

            VTools.IucScanner Iscan = ucScanner1;

         //   openFileDialog1.FileOk += OpenFileDialog1_FileOk;


           // sendFileBtn.Click += SendFileBtn_Click;

            ping.Click += delegate
              {
                  Iscan.SendContent = "<Ping>\n";
              };

            resetHostBtn.Click += delegate
              {
                  receiverRTB.ResetText();
                  Iscan.SendContent = "<Reset>\n";
              };
            EventHandler clear1 = delegate
              {
                  transmitterRTB.ResetText();
                  lastIndex = 0;
              };
            EventHandler clear2 = delegate
            {
                lastIndex = 0;
                receiverRTB.ResetText();
            };
            this.clearBtn1.Click += clear1;
            this.clearBtn2.Click += clear2;

      
            EventHandler transmit = delegate

            {
                if (transmitterRTB.Text.Length != 0)
                {
                    bool ok = transmitterRTB.Text.Last() == ' ';
                    ok = ok || transmitterRTB.Text.Last() == '\n';
                    ok = ok || transmitterRTB.Text.Last() == '.';
                    if (ok)
                    {
                        if (lastIndex < transmitterRTB.Text.Length)
                        {
                            string tosend = transmitterRTB.Text.Substring(lastIndex);
                            Iscan.SendContent = tosend;
                        }
                        lastIndex = transmitterRTB.Text.Length ;
                    }
                }
            };

            transmitterRTB.TextChanged += transmit;


            EventHandler handler = delegate
            {
                SetControlText(receiverRTB, Iscan.Result);
            };

            Iscan.MakeScanner(ref handler);

            Iscan.OpenPort = true;
        }
    }
}