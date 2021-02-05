using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using appweb.Models;
using System.Device.Gpio;
using System.Threading;
using Iot.Device.DHTxx;




namespace appweb.Controllers
{
    public class HomeController : Controller
    {
        private const int in1 = 23;
        private const int in2 = 24;
        private const int in3 = 27;
        private const int in4 = 17;
        private const int ena = 25;
        private const int enb = 22;

        private const int pin1 = 18;

        private const int tit = 21;

        private const int buzzer = 20;

        private const int led1 = 13;
        private const int led2 = 26;

        private const int fan = 16;

        private readonly GpioController controller;
       
        
        bool lastOn = false;
        bool control = false;


        public HomeController()
        {
           
            controller = new GpioController();
            //verilen pini çıktı verecek şekilde tanımlıyoruz.
            controller.OpenPin(in1, PinMode.Output);
            controller.OpenPin(in2, PinMode.Output);
            controller.OpenPin(in3, PinMode.Output);
            controller.OpenPin(in4, PinMode.Output);
            controller.OpenPin(ena, PinMode.Output);
            controller.OpenPin(enb, PinMode.Output);

            controller.OpenPin(pin1, PinMode.Input);

            controller.OpenPin(tit, PinMode.Input);

            controller.OpenPin(buzzer, PinMode.Output);

            controller.OpenPin(led1, PinMode.Output);
            controller.OpenPin(led2, PinMode.Output);

            controller.OpenPin(fan, PinMode.Output);

            controller.Write(in1, PinValue.Low);
            controller.Write(in2, PinValue.Low);
            controller.Write(in3, PinValue.Low);
            controller.Write(in4, PinValue.Low);
            controller.Write(ena, PinValue.High);
            controller.Write(enb, PinValue.High);

        }

        public IActionResult Index()
        {
            bool pinOn = controller.Read(pin1) == true;
            if (lastOn == pinOn)
            {
                ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
                ViewBag.Message = "!!! YOLDA ENGEL VAR !!!";
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.Low);
            }

                return View();
        }
        public IActionResult fanOn()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            controller.Write(fan, PinValue.High);
            return View("Index");
        }

        public IActionResult fanOff()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            controller.Write(fan, PinValue.Low);
            return View("Index");
        }

        public IActionResult SifreGir()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifreGir(sifreModel sifre)
        {
            string sifreId = sifre.sifreId;

            if (sifreId == "1234")
            {
                ViewBag.sifre = "ŞİFRE DOĞRU GİRİLDİ...";
            }
            else
            {
                ViewBag.sifre2 = "ŞİFRE YANLIŞ";
            }

            return View();
        }

        public IActionResult Yesil()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            controller.Write(led1, PinValue.High);
            controller.Write(led2, PinValue.Low);

            return View("Index");
        }

        public IActionResult Kırmızı()
        {
            controller.Write(led1, PinValue.Low);
            controller.Write(led2, PinValue.High);


            return View("SifreGir");
        }

        public IActionResult DontStop()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            bool pinOn = controller.Read(pin1) == true;
            if (lastOn == pinOn)
            {
                
                controller.Write(in1, PinValue.High);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.High);
                controller.Write(in4, PinValue.Low);
            }
            return View("Index");
        }

        public IActionResult PleaseStop()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            bool pinOn = controller.Read(pin1) == true;
            if (lastOn == pinOn)
            {
               
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.Low);
            }
            return View("Index");
        }

        public IActionResult HirsizAlarm()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            ViewBag.alarm = "Hırsız alarmı aktif!";
            
            bool titOn = controller.Read(tit) == true;

            if (control != titOn)
            {
                ViewBag.alarmdurumu1 = "UYARI: HIRSIZ VAR!!!";
                controller.Write(buzzer, PinValue.High);
                Thread.Sleep(2000);
                controller.Write(buzzer, PinValue.Low);

            }
            return View("Index");

        }

        
        public IActionResult BuzzerOn()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            controller.Write(buzzer, PinValue.High);
            return View("Index");
        }

        public IActionResult BuzzerOff()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            controller.Write(buzzer, PinValue.Low);
            return View("Index");
        }

        public IActionResult Forward()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            bool pinOn = controller.Read(pin1) == true;
            if (lastOn != pinOn)
            {
                controller.Write(in1, PinValue.High);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.High);
                controller.Write(in4, PinValue.Low);
                ViewBag.btnDurumu = "İLERİ GİDİYORSUNUZ!..";
            }
            else
            {
                ViewBag.Message = "!!! YOLDA ENGEL VAR !!!";
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.Low);
            }

            using (Dht11 dht = new Dht11(14))
            {
                var TempValue = dht.Temperature;
                if (TempValue.DegreesCelsius > 23)
                {
                    ViewBag.dht = $"Sıcaklık: {TempValue.DegreesCelsius:0.0#}\u00B0C";
                }
            }
            return View("Index");
        }

        public IActionResult Backward()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            bool pinOn = controller.Read(pin1) == true;

            if (lastOn != pinOn)
            {
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.High);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.High);
                ViewBag.btnDurumu = "GERİ GİDİYORSUNUZ!..";
            }
            else
            {
                ViewBag.Message = "!!! YOLDA ENGEL VAR !!!";
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.Low);
            }
            using (Dht11 dht = new Dht11(14))
            {
                var TempValue = dht.Temperature;
                if (TempValue.DegreesCelsius > 23)
                {
                    ViewBag.dht = $"Sıcaklık: {TempValue.DegreesCelsius:0.0#}\u00B0C";
                }
            }
            return View("Index");
        }

        public IActionResult Left()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            bool pinOn = controller.Read(pin1) == true;
            if (lastOn != pinOn)
            {
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.High);
                controller.Write(in4, PinValue.Low);
                ViewBag.btnDurumu = "SOLA DÖNDÜNÜZ!..";
            }
            else
            {

                ViewBag.Message = "!!! YOLDA ENGEL VAR !!!";
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.Low);
            }
            using (Dht11 dht = new Dht11(14))
            {
                var TempValue = dht.Temperature;
                if (TempValue.DegreesCelsius > 23)
                {
                    ViewBag.dht = $"Sıcaklık: {TempValue.DegreesCelsius:0.0#}\u00B0C";
                }
            }
            return View("Index");
        }

        public IActionResult Right()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            bool pinOn = controller.Read(pin1) == true;

            if (lastOn != pinOn)
            {
                controller.Write(in1, PinValue.High);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.Low);
                ViewBag.btnDurumu = "SAĞA DÖNDÜNÜZ!..";
            }    
            else
            {
                ViewBag.Message = "!!! YOLDA ENGEL VAR !!!";
                controller.Write(in1, PinValue.Low);
                controller.Write(in2, PinValue.Low);
                controller.Write(in3, PinValue.Low);
                controller.Write(in4, PinValue.Low);
            }
            using (Dht11 dht = new Dht11(14))
            {
                var TempValue = dht.Temperature;
                if (TempValue.DegreesCelsius > 23)
                {
                    ViewBag.dht = $"Sıcaklık: {TempValue.DegreesCelsius:0.0#}\u00B0C";
                }
            }
            return View("Index");
        }
        public IActionResult Stop()
        {
            ViewBag.yesil = "ARABAYI ÇALIŞTIRABİLİRSİNİZ...";
            bool pinOn = controller.Read(pin1) == true;
            if (lastOn != pinOn)
            {
            controller.Write(in1, PinValue.Low);
            controller.Write(in2, PinValue.Low);
            controller.Write(in3, PinValue.Low);
            controller.Write(in4, PinValue.Low);
            ViewBag.btnDurumu = "DURDUNUZ!..";
            }
            else
            {
                ViewBag.Message = "!!! YOLDA ENGEL VAR !!!";
            }
            using (Dht11 dht = new Dht11(14))
            {
                var TempValue = dht.Temperature;
                if (TempValue.DegreesCelsius > 23)
                {
                    ViewBag.dht = $"Sıcaklık: {TempValue.DegreesCelsius:0.0#}\u00B0C";
                }
            }

            return View("Index");
        }

        /*
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */
    }
}
