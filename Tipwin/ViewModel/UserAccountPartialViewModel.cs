using System;

namespace Tipwin.ViewModel
{
    public class UserNameData
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Oslovljavanje { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        // public AddressData AddressD { get; set; }
        //public ContactData ContactD { get; set; }
        // }

        //public class AddressData
        //{
        public string Ulica { get; set; }
        public string KucniBroj { get; set; }
        public int PostanskiBroj { get; set; }
        public string GradMjesto { get; set; }
        public string Drzava { get; set; }
        //}

        //public class ContactData
        //{
        public string JezikZaKontakt { get; set; }
        public int BrojTelefona { get; set; }
        public int BrojMobitela { get; set; }
    }
}
