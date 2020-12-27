using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientApp
{
    class Program
    {
        //static string portPrefix = "50"; //iis express
        static string portPrefix = "40"; //docker

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Please specify the process.");
                    Console.WriteLine("1 for list contact");
                    Console.WriteLine("2 for add contact");
                    Console.WriteLine("3 for remove contact");
                    Console.WriteLine("4 for add communication info");
                    Console.WriteLine("5 for remove communication info");
                    Console.WriteLine("6 for view full contact info");
                    Console.WriteLine("7 for create report");
                    Console.WriteLine("8 for view report list");
                    Console.WriteLine("9 for view report detail");
                    Console.WriteLine("10 for create bulk data (Name1, Surname1, FirmName1, ... )");
                    Console.WriteLine("Q for exit");

                    Console.Write("\nProcess Type: ");
                    var process = Console.ReadLine();
                    if (process.ToUpper() == "Q") break;

                    switch (process)
                    {
                        case "1": { getContactList(true); }; break;
                        case "2": { addContact(); }; break;
                        case "3": { removeContact(); }; break;
                        case "4": { addCommunication(); }; break;
                        case "5": { removeCommunication(); }; break;
                        case "6": { getContact(true); }; break;
                        case "7": { createReport(); }; break;
                        case "8": { getReportList(true); }; break;
                        case "9": { getReport(); }; break;
                        case "10": { createBulkData(); }; break;
                        default: break;
                    }

                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured: " + ex.Message);
                }
            }
        }

        static Contact getContact(bool print)
        {
            var list = getContactList(true);
            int seqNum = getIndex(list.Select(x => x.clientSideId).ToList(), "contact");

            var url = "http://localhost:" + portPrefix + "100/Directory/GetContact?ID=" + list.Where(x => x.clientSideId == seqNum).FirstOrDefault().ID.ToString();
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var contact = JsonSerializer.Deserialize<Contact>(result);
                Console.WriteLine("\nID: " + contact.ID + "\tName: " + contact.Name + "\tSurname: " + contact.Surname + "\tFirm Name: " + contact.FirmName);
                contact.CommunicationList = contact.CommunicationList ?? new List<Communication>();
                int index = 0;
                foreach (var item in contact.CommunicationList)
                {
                    item.clientSideId = ++index;
                    Console.WriteLine("\tSequence No: " + item.clientSideId + "\tType: " + item.Type + "\tContent: " + item.Content);
                }
                if (print)
                {
                    Console.WriteLine("\nProcess completed.");
                }
                return contact;
            }
        }

        static List<Contact> getContactList(bool print)
        {
            var list = new List<Contact>();
            var url = "http://localhost:" + portPrefix + "100/Directory/GetContactList";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                list = JsonSerializer.Deserialize<List<Contact>>(result);

                int index = 0;
                foreach (var item in list)
                {
                    item.clientSideId = ++index;
                    if (print)
                    {
                        Console.WriteLine("Sequence No: " + item.clientSideId + "\tName: " + item.Name + "\tSurname: " + item.Surname + "\tFirm Name: " + item.FirmName);
                    }
                }
            }

            return list;
        }

        static void addContact()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("Surname: ");
            var surname = Console.ReadLine();
            Console.Write("Firm Name: ");
            var firmname = Console.ReadLine();

            var url = "http://localhost:" + portPrefix + "100/Directory/AddContact";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var contact = new Contact { Name = name, Surname = surname, FirmName = firmname };
            var data = JsonSerializer.Serialize(contact);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Contact item = JsonSerializer.Deserialize<Contact>(result);
                Console.WriteLine("ID: " + item.ID + "\tName: " + item.Name + "\tSurname: " + item.Surname + "\tFirm Name: " + item.FirmName);
            }
            Console.WriteLine("\nProcess completed.");
        }

        static void removeContact()
        {
            var list = getContactList(true);
            int seqNum = getIndex(list.Select(x => x.clientSideId).ToList(), "contact");

            var url = "http://localhost:" + portPrefix + "100/Directory/RemoveContact?ID=" + list.Where(x => x.clientSideId == seqNum).FirstOrDefault().ID.ToString();
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine("\nProcess completed.");
            }
        }

        static void addCommunication()
        {
            var contact = getContact(false);
            Console.WriteLine("1. Phone");
            Console.WriteLine("2. Email");
            Console.WriteLine("3. Country");
            var type = getIndex(new List<int> { 1, 2, 3 }, "communication type").ToString();

            Console.Write("Please enter the content of communication: ");
            var content = Console.ReadLine();

            var url = "http://localhost:" + portPrefix + "100/Directory/AddCommunication";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var comm = new Communication { ContactID = contact.ID, Type = type, Content = content };
            var data = JsonSerializer.Serialize(comm);
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Communication item = JsonSerializer.Deserialize<Communication>(result);
                Console.WriteLine("\nProcess completed.");
            }
        }

        static void removeCommunication()
        {
            var contact = getContact(false);
            int seqNum = getIndex(contact.CommunicationList.Select(x => x.clientSideId).ToList(), "communication type");

            var url = "http://localhost:" + portPrefix + "100/Directory/RemoveCommunication?ID=" + contact.CommunicationList.Where(x => x.clientSideId == seqNum).FirstOrDefault().ID.ToString();
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine("\nProcess completed.");
            }
        }

        static int getIndex(List<int> list, string text)
        {
            int seqNum = 0;
            while (true)
            {
                try
                {
                    Console.Write("\nPlease enter the sequence no of " + text + ": ");
                    seqNum = Convert.ToInt32(Console.ReadLine());

                    if (list.Contains(seqNum)) break;
                    else
                    {
                        Console.WriteLine("Id is out of range");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter only number");
                }
            }
            return seqNum;
        }

        static void createReport()
        {
            var url = "http://localhost:" + portPrefix + "100/Directory/GetReport";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.GetResponse();
            Console.WriteLine("\nProcess completed.");
        }

        static List<Report> getReportList(bool print)
        {
            var list = new List<Report>();
            var url = "http://localhost:" + portPrefix + "500/Report/GetReportList";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                list = JsonSerializer.Deserialize<List<Report>>(result);

                int index = 0;
                foreach (var item in list)
                {
                    item.clientSideId = ++index;
                    Console.WriteLine("Sequence No: " + item.clientSideId + "\tReport Date: " + item.Date + "\tStatus: " + item.Status);
                }
            }
            if (print)
            {
                Console.WriteLine("\nProcess completed.");
            }

            return list;
        }

        static void getReport()
        {
            var list = getReportList(false);
            int seqNum = getIndex(list.Select(x => x.clientSideId).ToList(), "report");

            var url = "http://localhost:" + portPrefix + "500/Report/GetReport?ID=" + list.Where(x => x.clientSideId == seqNum).FirstOrDefault().ID.ToString();
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var reportDetail = JsonSerializer.Deserialize<List<ReportDetail>>(result);
                Console.WriteLine("\nOK");
                foreach (var item in reportDetail)
                {
                    Console.WriteLine("ID: " + String.Format("{0,-10}", item.ID) + " Location: " + String.Format("{0,-45}", item.Location) + " Contact Count: " + item.ContactCount + "\tPhone Count: " + item.PhoneCount);
                }
            }
            Console.WriteLine("\nProcess completed.");
        }

        static void createBulkData()
        {
            int count;
            while (true)
            {
                try
                {
                    Console.Write("Please enter record count: ");
                    count = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter only number");
                }
            }
            Console.WriteLine("Processing...");

            int _count = getContactList(false).Count;
            int i = _count;
            _count += count;
            for (; i < _count; i++)
            {
                var url = "http://localhost:" + portPrefix + "100/Directory/AddContact";
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";
                var contact = new Contact { Name = "Name_" + (i + 1), Surname = "Surname_" + (i + 1), FirmName = "FirmName_" + (i + 1) };
                var data = JsonSerializer.Serialize(contact);
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream())) { streamWriter.Write(data); }
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    contact = JsonSerializer.Deserialize<Contact>(result);
                }

                int totalC = new Random().Next(3, 9);
                for (int j = 0; j < totalC; j++)
                {
                    int typeC = j < 3 ? j + 1 : new Random().Next(1, 4);
                    var comm = new Communication(contact, typeC.ToString());
                    url = "http://localhost:" + portPrefix + "100/Directory/AddCommunication";
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpRequest.Method = "POST";
                    httpRequest.ContentType = "application/json";
                    data = JsonSerializer.Serialize(comm);
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream())) { streamWriter.Write(data); }
                    httpRequest.GetResponse();
                }

            }
            Console.WriteLine("Process completed.");
        }
    }
    public class Report
    {
        public int clientSideId { get; set; }
        [JsonPropertyName("id")]
        public Guid ID { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
    public class ReportDetail
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("reportID")]
        public Guid ReportID { get; set; }
        [JsonPropertyName("location")]
        public string Location { get; set; }
        [JsonPropertyName("contactCount")]
        public int ContactCount { get; set; }
        [JsonPropertyName("phoneCount")]
        public int PhoneCount { get; set; }
    }
    public class Contact
    {
        public int clientSideId { get; set; }
        [JsonPropertyName("id")]
        public Guid ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("surname")]
        public string Surname { get; set; }
        [JsonPropertyName("firmName")]
        public string FirmName { get; set; }
        [JsonPropertyName("communicationList")]
        public List<Communication> CommunicationList { get; set; }
    }
    public class Communication
    {
        public Communication() { }
        public Communication(Contact contact, string type)
        {
            ContactID = contact.ID;
            Type = type;
            if (type == "1")
            {
                Content = "50" + DateTime.UtcNow.Ticks.ToString().Substring(8);
            }
            else if (type == "2")
            {
                Content = contact.Name + "." + contact.Surname + "@" + contact.Surname + ".com";
            }
            else if (type == "3")
            {
                Content = countryList[new Random().Next(countryList.Count)];
            }
        }

        public int clientSideId { get; set; }
        [JsonPropertyName("id")]
        public Guid ID { get; set; }
        [JsonPropertyName("contactId")]
        public Guid ContactID { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }

        private List<string> countryList = new List<string>
            {
                "Aruba",
                "Antigua and Barbuda",
                "United Arab Emirates",
                "Afghanistan",
                "Algeria",
                "Azerbaijan",
                "Albania",
                "Armenia",
                "Andorra",
                "Angola",
                "American Samoa",
                "Argentina",
                "Australia",
                "Ashmore and Cartier Islands",
                "Austria",
                "Anguilla",
                "Åland Islands",
                "Antarctica",
                "Bahrain",
                "Barbados",
                "Botswana",
                "Bermuda",
                "Belgium",
                "Bahamas, The",
                "Bangladesh",
                "Belize",
                "Bosnia and Herzegovina",
                "Bolivia",
                "Myanmar",
                "Benin",
                "Belarus",
                "Solomon Islands",
                "Navassa Island",
                "Brazil",
                "Bassas da India",
                "Bhutan",
                "Bulgaria",
                "Bouvet Island",
                "Brunei",
                "Burundi",
                "Canada",
                "Cambodia",
                "Chad",
                "Sri Lanka",
                "Congo, Republic of the",
                "Congo, Democratic Republic of the",
                "China",
                "Chile",
                "Cayman Islands",
                "Cocos (Keeling) Islands",
                "Cameroon",
                "Comoros",
                "Colombia",
                "Northern Mariana Islands",
                "Coral Sea Islands",
                "Costa Rica",
                "Central African Republic",
                "Cuba",
                "Cape Verde",
                "Cook Islands",
                "Cyprus",
                "Denmark",
                "Djibouti",
                "Dominica",
                "Jarvis Island",
                "Dominican Republic",
                "Dhekelia Sovereign Base Area",
                "Ecuador",
                "Egypt",
                "Ireland",
                "Equatorial Guinea",
                "Estonia",
                "Eritrea",
                "El Salvador",
                "Ethiopia",
                "Europa Island",
                "Czech Republic",
                "French Guiana",
                "Finland",
                "Fiji",
                "Falkland Islands (Islas Malvinas)",
                "Micronesia, Federated States of",
                "Faroe Islands",
                "French Polynesia",
                "Baker Island",
                "France",
                "French Southern and Antarctic Lands",
                "Gambia, The",
                "Gabon",
                "Georgia",
                "Ghana",
                "Gibraltar",
                "Grenada",
                "Guernsey",
                "Greenland",
                "Germany",
                "Glorioso Islands",
                "Guadeloupe",
                "Guam",
                "Greece",
                "Guatemala",
                "Guinea",
                "Guyana",
                "Gaza Strip",
                "Haiti",
                "Hong Kong",
                "Heard Island and McDonald Islands",
                "Honduras",
                "Howland Island",
                "Croatia",
                "Hungary",
                "Iceland",
                "Indonesia",
                "Isle of Man",
                "India",
                "British Indian Ocean Territory",
                "Clipperton Island",
                "Iran",
                "Israel",
                "Italy",
                "Cote d'Ivoire",
                "Iraq",
                "Japan",
                "Jersey",
                "Jamaica",
                "Jan Mayen",
                "Jordan",
                "Johnston Atoll",
                "Juan de Nova Island",
                "Kenya",
                "Kyrgyzstan",
                "Korea, North",
                "Kingman Reef",
                "Kiribati",
                "Korea, South",
                "Christmas Island",
                "Kuwait",
                "Kosovo",
                "Kazakhstan",
                "Laos",
                "Lebanon",
                "Latvia",
                "Lithuania",
                "Liberia",
                "Slovakia",
                "Palmyra Atoll",
                "Liechtenstein",
                "Lesotho",
                "Luxembourg",
                "Libyan Arab",
                "Madagascar",
                "Martinique",
                "Macau",
                "Moldova, Republic of",
                "Mayotte",
                "Mongolia",
                "Montserrat",
                "Malawi",
                "Montenegro",
                "The Former Yugoslav Republic of Macedonia",
                "Mali",
                "Monaco",
                "Morocco",
                "Mauritius",
                "Midway Islands",
                "Mauritania",
                "Malta",
                "Oman",
                "Maldives",
                "Mexico",
                "Malaysia",
                "Mozambique",
                "New Caledonia",
                "Niue",
                "Norfolk Island",
                "Niger",
                "Vanuatu",
                "Nigeria",
                "Netherlands",
                "No Man's Land",
                "Norway",
                "Nepal",
                "Nauru",
                "Suriname",
                "Netherlands Antilles",
                "Nicaragua",
                "New Zealand",
                "Paraguay",
                "Pitcairn Islands",
                "Peru",
                "Paracel Islands",
                "Spratly Islands",
                "Pakistan",
                "Poland",
                "Panama",
                "Portugal",
                "Papua New Guinea",
                "Palau",
                "Guinea-Bissau",
                "Qatar",
                "Reunion",
                "Serbia",
                "Marshall Islands",
                "Saint Martin",
                "Romania",
                "Philippines",
                "Puerto Rico",
                "Russia",
                "Rwanda",
                "Saudi Arabia",
                "Saint Pierre and Miquelon",
                "Saint Kitts and Nevis",
                "Seychelles",
                "South Africa",
                "Senegal",
                "Saint Helena",
                "Slovenia",
                "Sierra Leone",
                "San Marino",
                "Singapore",
                "Somalia",
                "Spain",
                "Saint Lucia",
                "Sudan",
                "Svalbard",
                "Sweden",
                "South Georgia and the Islands",
                "Syrian Arab Republic",
                "Switzerland",
                "Trinidad and Tobago",
                "Tromelin Island",
                "Thailand",
                "Tajikistan",
                "Turks and Caicos Islands",
                "Tokelau",
                "Tonga",
                "Togo",
                "Sao Tome and Principe",
                "Tunisia",
                "East Timor",
                "Turkey",
                "Tuvalu",
                "Taiwan",
                "Turkmenistan",
                "Tanzania, United Republic of",
                "Uganda",
                "United Kingdom",
                "Ukraine",
                "United States",
                "Burkina Faso",
                "Uruguay",
                "Uzbekistan",
                "Saint Vincent and the Grenadines",
                "Venezuela",
                "British Virgin Islands",
                "Vietnam",
                "Virgin Islands (US)",
                "Holy See (Vatican City)",
                "Namibia",
                "West Bank",
                "Wallis and Futuna",
                "Western Sahara",
                "Wake Island",
                "Samoa",
                "Eswatini",
                "Serbia and Montenegro",
                "Yemen",
                "Zambia",
                "Zimbabwe"
            };
    }
}