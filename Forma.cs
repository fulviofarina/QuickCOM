using System;

using System.Windows.Forms;

namespace QuickCOM
{
    public partial class Forma : Form
    {
        private int lastIndex = 0;
        private delegate void refesher(RichTextBox control, string text);
        public void SetControlText(RichTextBox control, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new refesher(SetControlText), new object[] { control, text });
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

            // openFileDialog1.FileOk += OpenFileDialog1_FileOk;

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
                    string text = transmitterRTB.Text;
                    char lastChar = text[text.Length - 1];
                    bool ok = lastChar == ' ';
                    ok = ok || lastChar == '\n';
                    ok = ok || lastChar == '.';
                    if (ok)
                    {
                        if (lastIndex < text.Length)
                        {
                            string tosend = text.Substring(lastIndex);
                            Iscan.SendContent = tosend;
                        }
                        lastIndex = text.Length;
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