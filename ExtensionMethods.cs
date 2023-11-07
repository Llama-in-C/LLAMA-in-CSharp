namespace LLAMA_in_CSharp;

public class ExtensionMethods
{
    static void replace_all(ref string s, ref string search, ref string replace) {
        string result;
        for (size_t pos = 0; ; pos += search.length()) {
            auto new_pos = s.find(search, pos);
            if (new_pos == std::string::npos) {
                result += s.Substring(pos, s.size() - pos);
                break;
            }
            result += s.Substring(pos, new_pos - pos) + replace;
            pos = new_pos;
        }
        s = std::move(result);
    }
}