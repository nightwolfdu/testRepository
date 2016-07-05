using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_Miner.GameLogic;

namespace Windows_Miner
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            File.WriteAllText("res.txt", new GameSquare().ToString());
            Application.Run(new MinerForm());
        }
    }
}

public static class StringExtension
{
    public static string StrJoin(this IEnumerable<string> thisStrings, string delimeter)
    {
        var builder = new StringBuilder();
        var enumerator = thisStrings.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return "";
        }
        builder.Append(enumerator.Current);
        while (enumerator.MoveNext())
        {
            builder.Append(delimeter);
            builder.Append(enumerator.Current);
        }
        return builder.ToString();
    }
}

