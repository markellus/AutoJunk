using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Auto_Junk_Adder
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private  int count;
        private int runt;
        private string file;
        private string VarType;

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\run.bat"))
                File.Delete(Directory.GetCurrentDirectory() + "\\run.bat");

            MessageBox.Show("Made by Thaisen and Peatreat\nMake sure this program is in the same folder as your source.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK);
            string[] lines = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\", "*.cpp", SearchOption.AllDirectories);
            listBox1.Items.AddRange(lines);
            listBox2.Select();
            listBox2.Focus();
            label2.Text = "Count: " + count;
            label3.Text = "Total: " + listBox1.Items.Count;
            timer1.Interval = metroTrackBar3.Value;
            timer2.Interval = metroTrackBar3.Value;

            metroComboBox1.Items.Add("Int");
            metroComboBox1.Items.Add("Float");
            metroComboBox1.Items.Add("Long");
            metroComboBox1.Items.Add("Double");
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                if (metroComboBox1.Text == "")
                {
                    MessageBox.Show("Please choose a data type.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK);
                    return;
                }

                count = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("No \".cpp\" files were found.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK);
                return;
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            count = 0;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                string[] sln = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\", "*.sln", SearchOption.TopDirectoryOnly);
                listBox2.Items.AddRange(sln);
                if (listBox2.Items.Count > 0)
                {
                    string path = Directory.GetCurrentDirectory() + "\\run.bat";

                    if (!File.Exists(path))
                    {
                        using (var tw = new StreamWriter(path, true))
                        {
                            tw.WriteLine("@echo OFF ");
                            tw.WriteLine("echo *ONLY WORKS FOR VISUAL STUDIO 2017* ");
                            tw.WriteLine("set /p Product=Enter your Visual Studio Product (Community, Enterprise, Professional): ");
                            tw.WriteLine("call \"C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\%Product%\\VC\\Auxiliary\\Build\\vcvarsall.bat\" x86");
                            tw.WriteLine("echo Starting Build for all Projects with proposed changes");
                            tw.WriteLine("\n");
                            tw.WriteLine("set /p Solution=Enter your solution file (Name + Extension): ");
                            tw.WriteLine("devenv \"%~dp0%Solution% \" /build Release ");
                            tw.WriteLine("echo \n");
                            tw.WriteLine("echo All builds completed. ");
                            tw.WriteLine("pause");
                            tw.Close();
                        }
                    }

                    Process proc = null;
                    proc = new Process();
                    proc.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory() + "\\";
                    proc.StartInfo.FileName = "run.bat";
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    proc.WaitForExit();
                    if (File.Exists(Directory.GetCurrentDirectory() + "\\run.bat"))
                        File.Delete(Directory.GetCurrentDirectory() + "\\run.bat");
                }
                else
                {
                    MessageBox.Show("Place this program in the directory of your \".sln\" file before trying to build.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
                MessageBox.Show("Stop generating junk before trying to build.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                if (timer1.Enabled == false)
                {
                    count = 0;
                    timer2.Start();
                }
                else
                {
                    MessageBox.Show("Please stop generating junk before trying to remove junk.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("No \".cpp\" files were found.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void metroTrackBar1_Scroll_1(object sender, ScrollEventArgs e)
        {
            metroLabel2.Text = "Var Length: " + metroTrackBar1.Value;
        }

        private void metroTrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            metroLabel3.Text = "Func Length: " + metroTrackBar2.Value;
        }

        private void metroTrackBar3_Scroll(object sender, ScrollEventArgs e)
        {
            metroLabel4.Text = "Speed: " + metroTrackBar3.Value + " ms";
            timer1.Interval = metroTrackBar3.Value;
            timer2.Interval = metroTrackBar3.Value;
        }

        string Junk;

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void Write()
        {
            Random random = new Random();

            int[] randomNumbers = new int[201];

            for(int i = 0; i < 101; i++)
            {
                randomNumbers[i] = random.Next(1, 0x5f5e100);
            }
            // Seperator
            for (int i = 100; i < 201; i++)
            {
                randomNumbers[i] = random.Next(-1000000000, 0x5f5e100);
            }

            string FunctionName = RandomString(metroTrackBar2.Value);
            string VarName = RandomString(metroTrackBar1.Value);

            Junk = "// Junk Code By Troll Face & Thaisen's Gen, made less retarded by Markellus\n" +
            ("void " + FunctionName + randomNumbers[0] + "() ") +
            ("{ ");

            for(int i = 0; i < 100; i++)
            {
                Junk += "    " + VarType + " " + VarName + randomNumbers[i+1] + " = " + randomNumbers[i + 101] + ";" ;
            }
            for(int i = 0; i < 100; i += 2)
            {
                Junk += "    " + " " + VarName + randomNumbers[i + 1] + " = " + VarName + randomNumbers[i + 2] + ";";
            }


            Junk += ("}\n") + "// Junk Finished";

///////////////////////////////////////////////////////////////////////////////////////////////

            file = listBox1.Items[count].ToString();

            string existing = File.ReadAllText(file);
            string createText = existing + Environment.NewLine + Junk;

            File.WriteAllText(file, createText + Environment.NewLine);

            count = count + 1;
            label2.Text = "Count: " + count;
        }

        public void Delete()
        {
            string Pattern = "(// Junk Code By Troll Face & Thaisen's Gen)(.*?)(// Junk Finished)";
            Regex x = new Regex(Pattern, RegexOptions.Singleline);
            string file = listBox1.Items[count].ToString();
            string Text = File.ReadAllText(file);
            Text = x.Replace(Text, "");

            File.WriteAllText(file, Text);

            count = count + 1;
            label2.Text = "Count: " + count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count == listBox1.Items.Count)
            {
                count = 0;
                runt = runt + 1;
                label4.Text = "Passes: " + runt;
            }
            else
            {
                Write();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (count == listBox1.Items.Count)
            {
                count = 0;
                timer2.Stop();
                MessageBox.Show("Successfully removed all previous junk.", "Peatreat & Thaisen's Junk Generator", MessageBoxButtons.OK);
                return;
            }
            else
            {
                Delete();
            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            VarType = metroComboBox1.Text.ToLower();
        }
    }
}
