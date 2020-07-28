//using System.Linq;

namespace Discord_New
{
    public class CardData
    {
        public string Name { get; set; }

        public string Exp_m { get; set; }

        public string Exp_y { get; set; }

        public string Number { get; set; }

        public string Billing { get; set; }

        public override string ToString()
        {
            return string.Format("{0}\t{1}/{2}\t{3}\t{4}\r\n******************************\r\n", (object) Name,
                (object) Exp_m, (object) Exp_y, (object) Number, (object) Billing);
        }
    }
}