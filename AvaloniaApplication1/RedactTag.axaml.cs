using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Models;
using AvaloniaApplication1;
using AvaloniaApplication1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static AvaloniaApplication1.SavingDate;
namespace AvaloniaApplication1;

public partial class RedactTag : Window
{
    public List<ListTag> _AllClientss = Helper.user.ListTags.Where(x => x.IdClient == _RedClient.Id).ToList();
    public List<Tag> _AllTag = Helper.user.Tags.ToList();
    public RedactTag()
    {
        InitializeComponent();
        Name_Pers.Text = _RedClient.Name;
        Listins(_AllClientss); //����� ���� ����������� ��������
        Listins2(_AllTag);//��������� �����
    }

    private void Listins(List<ListTag> list)
    {


        TagList.ItemsSource = list.Select(x => new
        {
            tag[(int)x.IdTag - 1].Name,
            Color = $"{tag[(int)x.IdTag - 1].Color}",
            //tag[(int)y.IdTag - 1].Color,   
        });
    }
    private void Listins2(List<Tag> list)
    {


        TagList2.ItemsSource = list.Select(x => new
        {
            x.Name,
            x.Color,
            //tag[(int)y.IdTag - 1].Color,   
        });
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//��������� �����
    {
        new Redactor().Show();
        Close();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//���������� ������ ���� � ����� ������
    {
        if (TageName.Text != null && TageColor != null)
        {
            Tag newtag = new Tag();
            newtag.Id = SavingDate.tag.Count() + 1;
            newtag.Name = TageName.Text;
            newtag.Color = TageColor.Text;
            Helper.user.Tags.Add(newtag);
            Helper.user.SaveChanges();
            new RedactTag().Show();
            Close();

        }
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//���������� ���� �������
    {

        if (NumberCheck(TagNomer.Text) == false)
        {
            int v = Convert.ToInt32(TagNomer.Text);
            if (v > 0 && v <= SavingDate.tag.Count())
            {
                ListTag taglist = new ListTag();
                taglist.Id = SavingDate.listtag.Count() + 1;
                taglist.IdClient = _RedClient.Id;
                taglist.IdTag = v;
                Helper.user.ListTags.Add(taglist);
                Helper.user.SaveChanges();
                new RedactTag().Show();
                Close();

            }


        }
    }

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)//������� ��� � ������� 
    {
        if (NumberCheck(TagNomer1.Text) == false)
        {
            int v = Convert.ToInt32(TagNomer1.Text);
            if (v > 0 && v < SavingDate.tag.Count())
            {
                List<ListTag> Tag = Helper.user.ListTags.Where(x => x.IdClient == _RedClient.Id && x.IdTag == v).ToList();
                // int j= 

                Helper.user.ListTags.Remove(Helper.user.ListTags.Find(Tag.Last().Id)); //� �� ������� ������ � �������
                Helper.user.SaveChanges(); //���������� ���������
                new RedactTag().Show();
                Close();

            }


        }
    }
    private bool NumberCheck(string name)// �������� �� ������������ ������ �����
    {
        return new string[]
        { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
               "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z",
                "X", "C", "V", "B", "N", "M", "�", "�", "�", "�",
                 "�", "�", "�", "�", "�", "�", "�", "�", "�", "�",
                  "�", "�", "�", "�", "�", "�", "�", "�", "�", "�",
                   "�", "�", "�", "�", "�", "�", "�", "�",
                "`" , "~", "!", "@", "\"", "#", "�", "$", ";", "%",
                "^", ":" , "&", "?", "*", "_", "=",
                "'", "|", "/", "<", ">"
        }.Any(name.Contains);
    }
}