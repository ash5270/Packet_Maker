

namespace Packet_Maker
{
    public class ConfigData
    {
        public string output_dir { get ; set; }
        public string packet_file_path { get; set; }
        public string packet_base_file_path { get; set; }
        public IList<string> convert_language { get; set; }
    } 
}
