﻿using SpacerUnion.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpacerUnion.Windows
{
   
    public partial class MaterialFilterForm : Form
    {
        public List<MatFilter> filters;

        public Dictionary<string, uint> matAddr;

        public ConfirmForm formConf;

        public List<int> filderIndexToSaveList;


        public Dictionary<string, int> foundMatList;

        public MaterialFilterForm()
        {
            InitializeComponent();
            filters = new List<MatFilter>();
            matAddr = new Dictionary<string, uint>();
            filderIndexToSaveList = new List<int>();
            foundMatList = new Dictionary<string, int>();
            formConf = new ConfirmForm(null);
            
        }

        public void UpdateLang()
        {
            labelFilterListFiles.Text = Localizator.Get("WIN_MATFILTER_FILTERLIST_FILES");
            buttonFilterNew.Text = Localizator.Get("WIN_MATFILTER_FILTER_NEW");
            buttonFltRename.Text = Localizator.Get("WIN_MATFILTER_FILTERLIST_RENAME");
            buttonMatFilterSaveFilters.Text = Localizator.Get("WIN_MATFILTER_FILTERLIST_SAVE");


            this.Text = Localizator.Get("WIN_MATFILTER_FILTER_TITLE");
            tabControlMatFilter.TabPages[0].Text = Localizator.Get("WIN_MATFILTER_FILTER_TAB_MESH");
            tabControlMatFilter.TabPages[1].Text = Localizator.Get("WIN_MATFILTER_FILTER_TAB_VOBS");


            labelMatCountCurrentFilter.Text = Localizator.Get("WIN_MATFILTER_MATLIST_CURRENT") + listBoxMatList.Items.Count;



            labelMatFilterSeachInMats.Text = Localizator.Get("WIN_MATFILTER_FILTER_SEARCH_IN_MATS");
            groupBoxTexInfo.Text = Localizator.Get("WIN_MATFILTER_FILTER_TEXTURE");
            groupBoxMatSettings.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS");
            labelApplyFilter.Text = Localizator.Get("WIN_MATFILTER_FILTER_SET_FILTER");
            labelFilterApplyGroup.Text = Localizator.Get("WIN_MATFILTER_FILTER_SET_GROUP");
            buttonSavePML_File.Text = Localizator.Get("WIN_MATFILTER_FILTER_SAVE_FILTER");
            buttonApplyMatSettings.Text = Localizator.Get("BTN_APPLY");


            groupBoxFilterTexShowSettings.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_NAME");
            checkBoxTexImageUseAlpha.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_USE_ALPHA");
            checkBoxTexImageAlwaysCenter.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_USE_CENTER");
            checkBoxFilterTexAutoScale.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_USE_SCALE");


            labelTexSize.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_SIZE") + " -";
            labelTexAlpha.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_ALPHA") + " -";
            textBoxTexName.Text = String.Empty;

        }

        public void OnClose()
        {
            Imports.Extern_FilterMat_SaveFilters();
        }
        private void MaterialFilterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            OnClose();
        }

        public void OnFilterIndexChange(int index)
        {
            if (index == 0)
            {
                buttonFltRename.Enabled = false;
                buttonSavePML_File.Enabled = false;
            }
            else if (index != -1)
            {
                buttonFltRename.Enabled = true;
                buttonSavePML_File.Enabled = true;
            }
            else
            {
                //buttonFltRename.Enabled = false;
            }
        }

        private void listBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox != null)
            {
                int index = listBox.SelectedIndex;


                OnFilterIndexChange(index);

                if (index != -1)
                {
                    listBoxMatList.BeginUpdate();
                    listBoxMatList.Items.Clear();
                    //listBoxMatFilSearch.Items.Clear();
                    matAddr.Clear();

                    Imports.Stack_PushString(listBox.SelectedItem.ToString());
                    Imports.Extern_FillMatListByFilterName();

                    listBoxMatList.Sorted = true;
                    listBoxMatList.EndUpdate();

                    if (listBoxMatList.SelectedItem == null && listBoxMatList.Items.Count > 0)
                    {
                        listBoxMatList.SelectedIndex = 0;
                        groupBoxMatSettings.Enabled = true;
                        groupBoxFilterTexShowSettings.Enabled = true;
                    }
                    else
                    {
                        groupBoxMatSettings.Enabled = false;
                        groupBoxFilterTexShowSettings.Enabled = false;
                    }

                    labelMatCountCurrentFilter.Text = Localizator.Get("WIN_MATFILTER_MATLIST_CURRENT") + listBoxMatList.Items.Count.ToString();

                    //SearchMatInMatList();

                    if (listBoxMatList.Items.Count == 0)
                    {
                        if (pictureBoxTexture.Image != null)
                        {
                            pictureBoxTexture.Image.Dispose();
                            pictureBoxTexture.Image = null;
                        }

                        pictureBoxTexture.BackColor = Color.White;
                        labelTexSize.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_SIZE") + " -";
                        labelTexAlpha.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_ALPHA") + " -";
                        textBoxTexName.Text = String.Empty;
                    }
                }
                else
                {
                    if (pictureBoxTexture.Image != null)
                    {
                        pictureBoxTexture.Image.Dispose();
                        pictureBoxTexture.Image = null;
                    }

                    pictureBoxTexture.BackColor = Color.White;
                    labelTexSize.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_SIZE") + " -";
                    labelTexAlpha.Text = Localizator.Get("WIN_MATFILTER_FILTER_SETTINGS_ALPHA") + " -";
                    textBoxTexName.Text = String.Empty;
                }
                
            }
        }

        private void ProcessUsingLockbitsAndUnsafeAndParallel(Bitmap processedBitmap)
        {
           
        }

        [DllExport]
        public static void MatFilter_OnCreateNewMat()
        {
            string name = Imports.Stack_PeekString();

            SpacerNET.matFilterWin.listBoxFilter.SelectedIndex = Imports.Stack_PeekInt();
            SpacerNET.matFilterWin.listBoxFilter_SelectedIndexChanged(SpacerNET.matFilterWin.listBoxFilter, null);

            SpacerNET.matFilterWin.listBoxMatList.SelectedItem = name;
        }

        public void listBoxMatList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMatList.SelectedItem != null)
            {
                string text = listBoxMatList.GetItemText(listBoxMatList.SelectedItem);

                if (matAddr.ContainsKey(text))
                {
                    uint addr = matAddr[text];
                    Imports.Extern_SelectMat(addr);


                    comboBoxApplyFilter.BeginUpdate();
                    comboBoxApplyFilter.Items.Clear();

                    for (int i = 0; i < listBoxFilter.Items.Count; i++)
                    {
                        comboBoxApplyFilter.Items.Add(listBoxFilter.Items[i]);
                    }

                    //ConsoleEx.WriteLineYellow("listBoxMatList_SelectedIndexChanged");

                    UpdateMatGroupAndFilter();



                    comboBoxApplyFilter.EndUpdate();

                   

                    groupBoxMatSettings.Enabled = true;
                    groupBoxFilterTexShowSettings.Enabled = true;



                }
            }
            else
            {
                groupBoxMatSettings.Enabled = false;
                groupBoxFilterTexShowSettings.Enabled = false;
            }
            
        }

        private void comboBoxApplyFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxApplyGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void UpdateMatGroupAndFilter()
        {
            comboBoxApplyFilter.SelectedIndex = Imports.Extern_FillMat_GetCurrentMat_FilterIndex();
            comboBoxApplyGroup.SelectedIndex = Imports.Extern_FillMat_GetCurrentMat_GroupIndex();
        }

        private void buttonApplyMatSettings_Click(object sender, EventArgs e)
        {
            int libFlag = comboBoxApplyFilter.SelectedIndex;
            int group = comboBoxApplyGroup.SelectedIndex;

            int index = listBoxFilter.SelectedIndex;

            if (index != -1)
            {
                if (!filderIndexToSaveList.Contains(index))
                {
                    filderIndexToSaveList.Add(index);
                }

                if (!filderIndexToSaveList.Contains(libFlag))
                {
                    filderIndexToSaveList.Add(libFlag);
                }

            }
            

            Imports.Extern_FillMat_ApplyFilterAndGroup(libFlag, group);

            UpdateMatGroupAndFilter();
        }

        private void buttonMatFilterSaveAll_Click(object sender, EventArgs e)
        {
            Imports.Extern_FilterMat_SaveFilters();

        }

        
        public void OnRenameFilter(string name)
        {

            if (name.Length == 0)
            {
                MessageBox.Show(Localizator.Get("MSG_COMMON_NO_EMPTY_NAME"));
                return;
            }

            if (listBoxFilter.Items.Contains(name))
            {
                MessageBox.Show(Localizator.Get("MSG_COMMON_NO_UNIQUE_NAME"));
                return;
            }

            if (!Utils.IsOnlyLatin(name) && Utils.IsOptionActive("checkBoxOnlyLatinInInput"))
            {
                MessageBox.Show(Localizator.Get("FORM_ENTER_BAD_STRING_INPUT"));
                return;
            }

            listBoxFilter.Items[listBoxFilter.SelectedIndex] = name;

            Imports.Stack_PushString(name);
            Imports.Stack_PushInt(listBoxFilter.SelectedIndex);
            Imports.Extern_MatFilter_RenameFilter();



            Imports.Extern_FilterMat_SaveFilters();

            Imports.Stack_PushInt(listBoxFilter.SelectedIndex);
            Imports.Extern_MatFilter_SaveCurrentFilter();


            listBoxFilter_SelectedIndexChanged(listBoxFilter, null);


        }
        public void OnCreateNewFilter(string name)
        {

            if (name.Length == 0)
            {
                MessageBox.Show(Localizator.Get("MSG_COMMON_NO_EMPTY_NAME"));
                return;
            }

            if (listBoxFilter.Items.Contains(name))
            {
                MessageBox.Show(Localizator.Get("MSG_COMMON_NO_UNIQUE_NAME"));
                return;
            }

            if (!Utils.IsOnlyLatin(name) && Utils.IsOptionActive("checkBoxOnlyLatinInInput"))
            {
                MessageBox.Show(Localizator.Get("FORM_ENTER_BAD_STRING_INPUT"));
                return;
            }

            listBoxFilter.Items.Add(name);

            listBoxFilter.SelectedItem = name;

            Imports.Stack_PushString(name);
            Imports.Extern_FillMat_AddNewFilter();

            comboBoxApplyFilter.BeginUpdate();
            comboBoxApplyFilter.Items.Clear();

            for (int i = 0; i < listBoxFilter.Items.Count; i++)
            {
                comboBoxApplyFilter.Items.Add(listBoxFilter.Items[i]);
            }

            ConsoleEx.WriteLineYellow("OnCreateNewFilter");

            UpdateMatGroupAndFilter();



            comboBoxApplyFilter.EndUpdate();

            listBoxFilter.Focus();

        }

        private void buttonFilterNew_Click(object sender, EventArgs e)
        {
          formConf.buttonConfirmNo.Text = Localizator.Get("WIN_COMPLIGHT_CLOSEBUTTON");
          formConf.buttonConfirmYes.Text = Localizator.Get("WIN_BTN_CONFIRM");
          formConf.labelTextShow.Text = Localizator.Get("MSG_MATFILTER_NEW_NAME");
          formConf.confType = "MATFILTER_NEWFILTER";

          formConf.ShowDialog();
        }


        public void SaveFilterChanges()
        {
            if (listBoxFilter.SelectedItem != null)
            {

                for (int i = 0; i < filderIndexToSaveList.Count; i++)
                {
                    int index = filderIndexToSaveList[i];

                    Imports.Stack_PushInt(index);
                    Imports.Extern_MatFilter_SaveCurrentFilter();
                    //ConsoleEx.WriteLineYellow("Save: " + index + " " + listBoxFilter.SelectedItem);
                }

                filderIndexToSaveList.Clear();


            }
        }

        private void buttonSavePML_File_Click(object sender, EventArgs e)
        {
            SaveFilterChanges();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBoxFilter.SelectedIndex != -1)
            {
                formConf.buttonConfirmNo.Text = Localizator.Get("WIN_COMPLIGHT_CLOSEBUTTON");
                formConf.buttonConfirmYes.Text = Localizator.Get("WIN_BTN_CONFIRM");
                formConf.labelTextShow.Text = Localizator.Get("MSG_MATFILTER_RENAME");
                formConf.confType = "MATFILTER_RENAME_FILTER";

                formConf.ShowDialog();
            }
        }


        public void SearchMatInMatList()
        {
            string text = textBoxFilterSearch.Text.Trim().ToUpper();

            if (text.Length > 0)
            {
                listBoxMatFilSearch.BeginUpdate();
                listBoxMatFilSearch.Items.Clear();
                foundMatList.Clear();

                Imports.Stack_PushString(text);
                Imports.Exter_MatFilter_SearchMatByName();
                /*
                for (int i = 0; i < matAddr.Count; i++)
                {
                    var entry = matAddr.ElementAt(i);

                    if (entry.Key == text || entry.Key.Contains(text))
                    {
                        listBoxMatFilSearch.Items.Add(entry.Key);
                    }

                }
                */
                listBoxMatFilSearch.Sorted = true;
                listBoxMatFilSearch.EndUpdate();
            }
        }
        private void textBoxFilterSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                SearchMatInMatList();
            }
        }

        private void listBoxMatFilSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMatFilSearch.SelectedIndex != -1)
            {
                string text = listBoxMatFilSearch.GetItemText(listBoxMatFilSearch.SelectedItem);

                if (listBoxMatList.Items.Contains(text))
                {
                    listBoxMatList.SelectedItem = text;
                }
                else
                {
                    string key = listBoxMatFilSearch.SelectedItem.ToString();

                    //foundMatList
                    if (foundMatList.ContainsKey(key))
                    {
                        //ConsoleEx.WriteLineYellow("key found: " + key);
                        int filterIndex = foundMatList[key];

                        if (filterIndex >= 0 && filterIndex < listBoxFilter.Items.Count)
                        {
                           // ConsoleEx.WriteLineYellow("Set index: " + filterIndex);
                            listBoxFilter.SelectedIndex = filterIndex;

                            listBoxFilter_SelectedIndexChanged(listBoxFilter, null);


                            if (listBoxMatList.Items.Contains(key))
                            {
                                listBoxMatList.SelectedItem = key;
                            }
                            

                        }
                    }

                }
            }
        }

        private void listBoxMatList_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (e.Button == MouseButtons.Right && lb.SelectedIndex != -1)
            {

                    string visual = lb.GetItemText(lb.SelectedItem);
                    Clipboard.SetText(visual);

            
                    Imports.Stack_PushString(Localizator.Get("COPYBUFFER") + ": " + visual);

                    Imports.Extern_PrintGreen();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        void Filter_UpdateTextureShowSettings()
        {
            Imports.Stack_PushInt(Convert.ToInt32(checkBoxTexImageUseAlpha.Checked));
            Imports.Stack_PushInt(Convert.ToInt32(checkBoxTexImageAlwaysCenter.Checked));
            Imports.Stack_PushInt(Convert.ToInt32(checkBoxFilterTexAutoScale.Checked));
            Imports.Extern_MatFilter_UpdateTextureShowSettings();

        }
        private void checkBoxTexImageUseAlpha_CheckedChanged(object sender, EventArgs e)
        {
            Filter_UpdateTextureShowSettings();
        }

        private void checkBoxTexImageAlwaysCenter_CheckedChanged(object sender, EventArgs e)
        {
            Filter_UpdateTextureShowSettings();
        }

        private void checkBoxFilterTexAutoScale_CheckStateChanged(object sender, EventArgs e)
        {
            Filter_UpdateTextureShowSettings();
        }

        private void buttonMatFilter_NewMat_Click(object sender, EventArgs e)
        {

            formConf.buttonConfirmNo.Text = Localizator.Get("WIN_COMPLIGHT_CLOSEBUTTON");
            formConf.buttonConfirmYes.Text = Localizator.Get("WIN_BTN_CONFIRM");
            formConf.labelTextShow.Text = Localizator.Get("MSG_MATFILTER_NEW_MAT_NAME");
            formConf.confType = "MATFILTER_NEWMATERIAL";

            formConf.ShowDialog();

           
        }

        private void textBoxFilterSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }

    public class MatFilter
    {
        public string name;
        public int id;
    }

}
