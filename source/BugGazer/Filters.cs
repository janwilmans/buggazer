using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugGazer
{
    public partial class Filters : Form
    {
        //ObservableCollection<Pattern> patternList = new ObservableCollection<Pattern>();
        List<Pattern> patternList = new List<Pattern>();
        List<TextColor> colorList = new List<TextColor>();
        List<FilterAction.Actions> actionList = new List<FilterAction.Actions>();
        List<FilterAction> filterActionList = new List<FilterAction>();

        public Filters()
        {
            InitializeComponent();

            patternList.Add(new Pattern("(?<key>\\w+):(?<value>\\w+)"));
            patternList.Add(new Pattern("(?<test>123.+)"));
            patternList.Add(new Pattern("(?<key>50\\w+)"));
            patternList_Changed();

            colorList.Add(new TextColor("Default"));
            colorList.Add(new TextColor("key"));
            colorList.Add(new TextColor("value"));
            colorList.Add(new TextColor("Brightred"));
            colorList.Add(new TextColor("Unimportant"));
            colorListView.SetObjects(colorList);
            colorDropBox.Items.AddRange(colorList.Select(c => c.Text).ToArray());
            colorDropBox.SelectedIndex = 1;

            actionDropBox.Items.AddRange(Enum.GetNames(typeof(FilterAction.Actions)));
            actionDropBox.SelectedIndex = 1;

            filterActionListView.EmptyListMsg = "No filters defined";
            filterActionListView.SetObjects(filterActionList);
            /*
            TextOverlay textOverlay = filterList.EmptyListMsgOverlay as TextOverlay;
            textOverlay.TextColor = Color.Firebrick;
            textOverlay.BackColor = Color.AntiqueWhite;
            textOverlay.BorderColor = Color.DarkRed;
            textOverlay.BorderWidth = 4.0f;
            textOverlay.Font = new Font("Chiller", 36);
            textOverlay.Rotation = -5;
             */

            //patternDropbox.read

        }

        void patternList_Changed()
        {
            patternListView.SetObjects(patternList);
            patternDropbox.Items.Clear();
            patternDropbox.Items.AddRange(patternList.Select(pattern => pattern.Text).ToArray());
            patternDropbox.SelectedIndex = 1;
        }

        private void removePatternButton_Click(object sender, EventArgs e)
        {
            if (patternListView.SelectedIndex < patternList.Count)
            {
                patternList.RemoveAt(patternListView.SelectedIndex);
            }
            patternList_Changed();
        }

        private void addPatternButton_Click(object sender, EventArgs e)
        {
            patternList.Add(new Pattern("<new>"));
            patternList_Changed();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            // patternList remove at items that contain "<new>" before writing changes.
            Close();
        }

        private void bgColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.ShowDialog();
            colorListView.Select();
        }

        private void addActionButton_Click(object sender, EventArgs e)
        {
            FilterAction action = new FilterAction();
            action.Enabled = true;
            action.Action = FilterAction.Parse((string)actionDropBox.SelectedItem);
            action.Pattern = new Pattern((string)patternDropbox.SelectedItem);
            action.TextColor = new TextColor((string)colorDropBox.SelectedItem);
            filterActionList.Add(action);
            filterActionListView.SetObjects(filterActionList);
            
            foreach (FilterAction f in filterActionList)
            {
                Controller.WriteLine("FilterAction");
                Controller.WriteLine("  Enabled: {0}", f.Enabled);
                Controller.WriteLine("  Pattern: {0}", f.Pattern.Text);
                Controller.WriteLine("  Action: {0}", f.Action);
                Controller.WriteLine("  TextColor: {0}", f.TextColor.Text);
            }
        }

        private void removeActionButton_Click(object sender, EventArgs e)
        {
            if (filterActionListView.SelectedIndex < filterActionList.Count)
            {
                filterActionList.RemoveAt(filterActionListView.SelectedIndex);
            }
            filterActionListView.SetObjects(filterActionList);
        }
    }

    class Pattern
    {
        public Pattern(string s)
        {
            Text = s;
        }

        public string Text;
    };

    class TextColor
    {
        public TextColor(string s)
        {
            Text = s;
        }

        public string Text;
    };

    class FilterAction
    {
        public enum Actions { None, Include, Exclude, Track };

        public static Actions Parse(string value)
        {
            Actions action;
            try
            {
                action = (Actions) Enum.Parse(typeof(Actions), value, true);
            }
            catch (Exception)
            {
                action = Actions.None;
            }
            return action;
        }

        public bool Enabled;
        public Pattern Pattern;
        public Actions Action;
        public TextColor TextColor;
    };

}
