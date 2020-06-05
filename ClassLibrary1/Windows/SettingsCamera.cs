﻿
using SpacerUnion.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SpacerUnion
{
    public partial class SettingsCamera : Form
    {
        public SettingsCamera()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }



        private void SettingsCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnApplySettings();
            Hide();
            e.Cancel = true;
            
        }
        
        private void OnApplySettings()
        {

            int value;

            int.TryParse(textBoxLimitFPS.Text.Trim(), out value);

            Imports.Stack_PushString("maxFPS");
            Imports.Extern_SetSetting(value);
            
        }
        
        public void UpdateAll()
        {
            trackBarTransSpeed_ValueChanged(null, EventArgs.Empty);
            trackBarRotSpeed_ValueChanged(null, EventArgs.Empty);
            trackBarWorld_ValueChanged(null, EventArgs.Empty);
            trackBarVobs_ValueChanged(null, EventArgs.Empty);

        }

        private void trackBarTransSpeed_ValueChanged(object sender, EventArgs e)
        {
            labelTrans.Text = "Скорость полета: " + trackBarTransSpeed.Value;
            textBoxCamTrans.Text = trackBarTransSpeed.Value.ToString();
            Imports.Stack_PushString("camTransSpeed");
            Imports.Extern_SetSetting(trackBarTransSpeed.Value);
            
        }

        private void trackBarRotSpeed_ValueChanged(object sender, EventArgs e)
        {
            labelRot.Text = "Скорость поворота: " + trackBarRotSpeed.Value;
            textBoxCamRot.Text = trackBarRotSpeed.Value.ToString();
            Imports.Stack_PushString("camRotSpeed");
            Imports.Extern_SetSetting(trackBarRotSpeed.Value);
            
        }

        private void trackBarWorld_ValueChanged(object sender, EventArgs e)
        {
            labelWorld.Text = "Мир: " + trackBarWorld.Value;
            textBoxWorldRange.Text = trackBarWorld.Value.ToString();
            Imports.Stack_PushString("rangeWorld");
            Imports.Extern_SetSetting(trackBarWorld.Value);
            
        }

        private void trackBarVobs_ValueChanged(object sender, EventArgs e)
        {
            labelVobs.Text = "Вобы: " + trackBarVobs.Value;
            textBoxVobsRange.Text = trackBarVobs.Value.ToString();
            Imports.Stack_PushString("rangeVobs");
            Imports.Extern_SetSetting(trackBarVobs.Value);
            
        }

        private void Применить_Click(object sender, EventArgs e)
        {
            OnApplySettings();

            this.Hide();
        }

        private void checkBoxFPS_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Imports.Stack_PushString("showFps");
            Imports.Extern_SetSetting(Convert.ToInt32(cb.Checked));
            
        }

        private void checkBoxTris_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Imports.Stack_PushString("showTris");
            Imports.Extern_SetSetting(Convert.ToInt32(cb.Checked));
            
        }

        private void checkBoxCamCoord_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Imports.Stack_PushString("showCamCoords");
            Imports.Extern_SetSetting(Convert.ToInt32(cb.Checked));
            
        }

        private void checkBoxVobs_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Imports.Stack_PushString("showVobsCount");
            Imports.Extern_SetSetting(Convert.ToInt32(cb.Checked));
            
        }

        private void checkBoxWaypoints_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Imports.Stack_PushString("showWaypointsCount");
            Imports.Extern_SetSetting(Convert.ToInt32(cb.Checked));
            
        }

        private void checkBoxDistVob_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Imports.Stack_PushString("showVobDist");
            Imports.Extern_SetSetting(Convert.ToInt32(cb.Checked));
            
        }

        private void checkBoxCameraHideWins_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            Imports.Stack_PushString("hideCamWindows");
            Imports.Extern_SetSetting(Convert.ToInt32(cb.Checked));
            
        }

        private void textBoxLimitFPS_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void SettingsCamera_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
         (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void SettingsCamera_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                SettingsCamera_FormClosing(null, new FormClosingEventArgs(CloseReason.UserClosing, true));
            }
        }

        private void textBoxCamTrans_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxCamTrans_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxCamTrans.Text.Trim();

            if (text.Length == 0)
            {
                return;
            }

            int value = Convert.ToInt32(text);

            if (value > trackBarTransSpeed.Maximum)
            {
                value = trackBarTransSpeed.Maximum;
            }
            if (value < trackBarTransSpeed.Minimum)
            {
                value = trackBarTransSpeed.Minimum;
            }

            trackBarTransSpeed.Value = value;


        }

        private void textBoxCamRot_TextChanged(object sender, EventArgs e)
        {

            string text = textBoxCamRot.Text.Trim();

            if (text.Length == 0)
            {
                return;
            }


            int value = Convert.ToInt32(text);

            if (value > trackBarRotSpeed.Maximum)
            {
                value = trackBarRotSpeed.Maximum;
            }
            if (value < trackBarRotSpeed.Minimum)
            {
                value = trackBarRotSpeed.Minimum;
            }

            trackBarRotSpeed.Value = value;
        }

        private void textBoxWorldRange_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxWorldRange.Text.Trim();

            if (text.Length == 0)
            {
                return;
            }

            int value = Convert.ToInt32(text);

            if (value > trackBarWorld.Maximum)
            {
                value = trackBarWorld.Maximum;
            }
            if (value < trackBarWorld.Minimum)
            {
                value = trackBarWorld.Minimum;
            }

            trackBarWorld.Value = value;
        }

        private void textBoxVobsRange_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxVobsRange.Text.Trim();

            if (text.Length == 0)
            {
                return;
            }


            int value = Convert.ToInt32(text);

            if (value > trackBarVobs.Maximum)
            {
                value = trackBarVobs.Maximum;
            }
            if (value < trackBarVobs.Minimum)
            {
                value = trackBarVobs.Minimum;
            }

            trackBarVobs.Value = value;
        }
    }
}
