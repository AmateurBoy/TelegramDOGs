﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace ConsoleTestGI
{
    public class ProccesAPI
    {
        public Process process;
        private static ProccesAPI Instance;
        private ProccesAPI() { }
        public  static ProccesAPI GetProccesAPI()
        {
            if (Instance == null)
                Instance = new ProccesAPI();      
            return Instance;            
        }
        public void StartServer()
        {
             if(process == null)
            process = Process.Start(@"C:\MAMP\MAMP.exe");
        }
        public void StopServer()
        {
            if (process != null)
            {
                Console.WriteLine("Отключение потока Сервера");
                process.Kill();
                process = null;
            }
            else
            {
                Console.WriteLine("Поток сервера не запущен,закрывать нечего");
            }
        }
        public void StatusServer()
        {
            bool SingChek = false;
            Process[] processes = Process.GetProcessesByName("MAMP");
            foreach (var item in processes)
            {
                SingChek = true;
                Console.WriteLine($"Найден процес:{item.ProcessName} id:{item.Id}");
                process = item;
            }
            if(SingChek==false)
            {
                Console.WriteLine("Повторная попытка запустить процес...");                
                StartServer();
                Thread.Sleep(3000);
                StatusServer();
            }    
        }
    }
}