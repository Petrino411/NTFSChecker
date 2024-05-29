
using System.Windows.Forms;

namespace NTFSChecker.Services;

public class MyLogger
{
    private ListBox _listLogs;
    public MyLogger(ListBox listBox)
    {
        _listLogs = listBox;
    }

    public void LogToUi(string message)
    {
        _listLogs.Items.Add(message);
    }

}