using PoEMap.Classes;

namespace PoEMap
{
    /// <summary>
    /// Reads the previous or already existing information (maps and ongoing ID for Apifetching) from files.
    /// </summary>
    public class ReadFile
    {
        public static Maplist ReadMapsFromFile()
        {
            Maplist maplist = new Maplist();
            // TODO: Read the existing maps from the file. / Read maps from database.
            return maplist;
        }

        public static string ReadNextIdFromFile()
        {
            string nextId = "";
            // TODO: Read the current ID for apifetching.
            return nextId;
        }
    }
}
