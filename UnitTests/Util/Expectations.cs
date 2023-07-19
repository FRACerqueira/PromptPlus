using System.Text;

namespace PPlus.Tests.Util
{
    internal static class Expectations
    {
        public static string GetVerifyAnsi(string file)
        {
            var pathfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"AnsiExpectations/{file}");
            var dataexpected = new StringBuilder();
            var aux = File.ReadAllLines(pathfile);
            for (int i = 0; i < aux.Length; i++)
            {
                if (i < aux.Length - 1)
                {
                    dataexpected.AppendLine(aux[i]);
                }
                else
                {
                    dataexpected.Append(aux[i]);
                }
            }
            return dataexpected.ToString();
        }

        public static string GetVerifyStd(string file)
        {
            var pathfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"StdExpectations/{file}");
            var dataexpected = new StringBuilder();
            var aux = File.ReadAllLines(pathfile);
            for (int i = 0; i < aux.Length; i++)
            {
                dataexpected.AppendLine(aux[i]);
            }
            return dataexpected.ToString();
        }
        public static string GetVerifyControlStd(string file)
        {
            var pathfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"StdExpectations/{file}");
            var dataexpected = new StringBuilder();
            var aux = File.ReadAllLines(pathfile);
            for (int i = 0; i < aux.Length; i++)
            {
                if (i < aux.Length - 1)
                {
                    dataexpected.AppendLine(aux[i]);
                }
                else
                {
                    dataexpected.Append(aux[i]);
                }
            }
            return dataexpected.ToString();
        }

    }
}
