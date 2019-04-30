using System;
using System.Collections.Generic;
using System.Text;

namespace CoVisiting.Service
{
    public class NumCaseService
    {
        public string GetCaseForSubsFromNum(int n)
        {
            switch (n % 100)
            {
                case 11: return "Учасників";
                case 12: return "Учасників";
                case 13: return "Учасників";
                case 14: return "Учасників";
                default: return SubCase(n);
            }
        }

        private string SubCase(int n)
        {
            switch (n % 10)
            {
                case 1: return "Учасник";
                case 2: return "Учасника";
                case 3: return "Учасника";
                case 4: return "Учасника";
                default: return "Учасників";
            }
        }


        public string GetCaseForEventsFromNum(int n)
        {
            switch (n % 100)
            {
                case 11: return "Подій";
                case 12: return "Подій";
                case 13: return "Подій";
                case 14: return "Подій";
                default: return EventCase(n);
            }
        }

        private string EventCase(int n)
        {
            switch (n % 10)
            {
                case 1: return "Подія";
                case 2: return "Події";
                case 3: return "Події";
                case 4: return "Події";
                default: return "Подій";
            }
        }


    }
}
