using CommandLine;
using System.Text;

namespace Base63 {
    class Options {
        [Option('e', "encode", Required = false, HelpText = "Encode a string using Base63.")]
        public string Encode { get; set; }

        [Option('d', "decode", Required = false, HelpText = "Decode a Base63-encoded string.")]
        public string Decode { get; set; }
    }
    class Program {
        static void Main(string[] args) {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options => {
                if (!string.IsNullOrWhiteSpace(options.Encode)) {
                    string encoded = Base63Encode(options.Encode);
                    Console.WriteLine("Base63 Encoded:\r\n" + encoded);
                }
                else if (!string.IsNullOrWhiteSpace(options.Decode)) {
                    string decoded = Base63Decode(options.Decode);
                    Console.WriteLine("Base63 Decoded:\r\n " + decoded);
                }
                else {
                    Console.WriteLine("Use the following commands to encode Base63 or decode Base63.\r\nEncode\r\nBase63.exe -e STRING\r\n\r\nDecode\r\nBase63.exe -d ENCODEDSTRING");
                }
            });
        }
        public static string Base63Decode(string input) {
            string s = input;
            s = s.Replace('-', '+');
            s = s.Replace('_', '/');
            switch (s.Length % 4) {
                case 0: break;
                case 2: s += "=="; break;
                case 3: s += "="; break;
                default: throw new System.Exception("String was not in the proper format.");
            }
            var b = Convert.FromBase64String(s);
            return Encoding.UTF8.GetString(b, 0, b.Length);
        }

        public static string Base63Encode(string input) {
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            string base64String = Convert.ToBase64String(byteArray);
            string base63Url = base64String.Replace('+', '-').Replace('/', '_');
            base63Url = base63Url.TrimEnd('=');
            return base63Url; ;
        }
    }
}