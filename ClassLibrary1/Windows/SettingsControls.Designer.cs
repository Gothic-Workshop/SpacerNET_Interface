﻿namespace SpacerUnion.Windows
{
    partial class SettingsControls
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxControlVob = new System.Windows.Forms.GroupBox();
            this.labelVobTrans = new System.Windows.Forms.Label();
            this.labelVobRot = new System.Windows.Forms.Label();
            this.trackBarVobTransSpeed = new System.Windows.Forms.TrackBar();
            this.trackBarVobRotSpeed = new System.Windows.Forms.TrackBar();
            this.buttonVobControlApply = new System.Windows.Forms.Button();
            this.groupBoxControlVob.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVobTransSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVobRotSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxControlVob
            // 
            this.groupBoxControlVob.Controls.Add(this.labelVobTrans);
            this.groupBoxControlVob.Controls.Add(this.labelVobRot);
            this.groupBoxControlVob.Controls.Add(this.trackBarVobTransSpeed);
            this.groupBoxControlVob.Controls.Add(this.trackBarVobRotSpeed);
            this.groupBoxControlVob.Location = new System.Drawing.Point(12, 12);
            this.groupBoxControlVob.Name = "groupBoxControlVob";
            this.groupBoxControlVob.Size = new System.Drawing.Size(298, 139);
            this.groupBoxControlVob.TabIndex = 9;
            this.groupBoxControlVob.TabStop = false;
            this.groupBoxControlVob.Text = "Управление вобом";
            // 
            // labelVobTrans
            // 
            this.labelVobTrans.AutoSize = true;
            this.labelVobTrans.Location = new System.Drawing.Point(6, 19);
            this.labelVobTrans.Name = "labelVobTrans";
            this.labelVobTrans.Size = new System.Drawing.Size(129, 13);
            this.labelVobTrans.TabIndex = 1;
            this.labelVobTrans.Text = "Скорость перемещения";
            // 
            // labelVobRot
            // 
            this.labelVobRot.AutoSize = true;
            this.labelVobRot.Location = new System.Drawing.Point(6, 69);
            this.labelVobRot.Name = "labelVobRot";
            this.labelVobRot.Size = new System.Drawing.Size(109, 13);
            this.labelVobRot.TabIndex = 2;
            this.labelVobRot.Text = "Скорость вращения";
            // 
            // trackBarVobTransSpeed
            // 
            this.trackBarVobTransSpeed.Location = new System.Drawing.Point(1, 35);
            this.trackBarVobTransSpeed.Maximum = 50000;
            this.trackBarVobTransSpeed.Minimum = 1;
            this.trackBarVobTransSpeed.Name = "trackBarVobTransSpeed";
            this.trackBarVobTransSpeed.Size = new System.Drawing.Size(235, 45);
            this.trackBarVobTransSpeed.TabIndex = 0;
            this.trackBarVobTransSpeed.TickFrequency = 500;
            this.trackBarVobTransSpeed.Value = 10000;
            this.trackBarVobTransSpeed.ValueChanged += new System.EventHandler(this.trackBarVobTransSpeed_ValueChanged);
            // 
            // trackBarVobRotSpeed
            // 
            this.trackBarVobRotSpeed.Location = new System.Drawing.Point(2, 88);
            this.trackBarVobRotSpeed.Maximum = 200;
            this.trackBarVobRotSpeed.Minimum = 1;
            this.trackBarVobRotSpeed.Name = "trackBarVobRotSpeed";
            this.trackBarVobRotSpeed.Size = new System.Drawing.Size(234, 45);
            this.trackBarVobRotSpeed.TabIndex = 3;
            this.trackBarVobRotSpeed.Value = 32;
            this.trackBarVobRotSpeed.ValueChanged += new System.EventHandler(this.trackBarVobRotSpeed_ValueChanged);
            // 
            // buttonVobControlApply
            // 
            this.buttonVobControlApply.Location = new System.Drawing.Point(93, 174);
            this.buttonVobControlApply.Name = "buttonVobControlApply";
            this.buttonVobControlApply.Size = new System.Drawing.Size(115, 23);
            this.buttonVobControlApply.TabIndex = 11;
            this.buttonVobControlApply.Text = "Применить";
            this.buttonVobControlApply.UseVisualStyleBackColor = true;
            this.buttonVobControlApply.Click += new System.EventHandler(this.buttonVobControlApply_Click);
            // 
            // SettingsControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 526);
            this.Controls.Add(this.buttonVobControlApply);
            this.Controls.Add(this.groupBoxControlVob);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsControls";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки управления";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsControls_FormClosing);
            this.groupBoxControlVob.ResumeLayout(false);
            this.groupBoxControlVob.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVobTransSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVobRotSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxControlVob;
        private System.Windows.Forms.Label labelVobTrans;
        private System.Windows.Forms.Label labelVobRot;
        public System.Windows.Forms.TrackBar trackBarVobTransSpeed;
        public System.Windows.Forms.TrackBar trackBarVobRotSpeed;
        private System.Windows.Forms.Button buttonVobControlApply;
    }
}