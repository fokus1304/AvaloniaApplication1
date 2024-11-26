using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Models;
using AvaloniaApplication1;
using AvaloniaApplication1.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static AvaloniaApplication1.SavingDate;
namespace AvaloniaApplication1;

public partial class HistoryAndFile : Window
{
    public List<File> _AllClientss23 = Helper.user.Files.Where(x => x.IdKlient == _RedClient.Id).ToList();
    public List<Posehenium> _AllClientss13 = Helper.user.Posehenia.Where(x => x.IdKlient == _RedClient.Id).ToList();
    public HistoryAndFile()
    {

        InitializeComponent();
        Name_Pers.Text = _RedClient.Name;
        Listins(_AllClientss23);
        Listins1(_AllClientss13);
    }
    private void Listins(List<File> list)
    {
        FileList.ItemsSource = list.Select(x => new
        {

            filelist[(int)x.IdSpisok - 1].File,

        });
    }
    private void Listins1(List<Posehenium> list)
    {
        PosList2.ItemsSource = list.Select(x => new
        {

            visits[(int)x.IdPosh - 1].Data,
            visits[(int)x.IdPosh - 1].Time,

        });
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}