﻿
namespace SpacerUnion.Windows
{
    partial class VobCatalogForm
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
            this.listBoxGroups = new System.Windows.Forms.ListBox();
            this.listBoxItems = new System.Windows.Forms.ListBox();
            this.buttonAddNewGroup = new System.Windows.Forms.Button();
            this.buttonRemoveSelected = new System.Windows.Forms.Button();
            this.buttonRenameSelected = new System.Windows.Forms.Button();
            this.buttonUP = new System.Windows.Forms.Button();
            this.buttonDOWN = new System.Windows.Forms.Button();
            this.groupBoxGroups = new System.Windows.Forms.GroupBox();
            this.groupBoxItems = new System.Windows.Forms.GroupBox();
            this.buttonAddElement = new System.Windows.Forms.Button();
            this.buttonRemoveItem = new System.Windows.Forms.Button();
            this.buttonUpRight = new System.Windows.Forms.Button();
            this.buttonDownRight = new System.Windows.Forms.Button();
            this.checkBoxVobCreateActive = new System.Windows.Forms.CheckBox();
            this.groupBoxGroups.SuspendLayout();
            this.groupBoxItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxGroups
            // 
            this.listBoxGroups.FormattingEnabled = true;
            this.listBoxGroups.Location = new System.Drawing.Point(33, 12);
            this.listBoxGroups.Name = "listBoxGroups";
            this.listBoxGroups.Size = new System.Drawing.Size(162, 173);
            this.listBoxGroups.TabIndex = 0;
            this.listBoxGroups.SelectedIndexChanged += new System.EventHandler(this.listBoxGroups_SelectedIndexChanged);
            // 
            // listBoxItems
            // 
            this.listBoxItems.FormattingEnabled = true;
            this.listBoxItems.Location = new System.Drawing.Point(201, 12);
            this.listBoxItems.Name = "listBoxItems";
            this.listBoxItems.Size = new System.Drawing.Size(339, 173);
            this.listBoxItems.TabIndex = 1;
            this.listBoxItems.SelectedIndexChanged += new System.EventHandler(this.listBoxItems_SelectedIndexChanged);
            this.listBoxItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxItems_MouseDown);
            // 
            // buttonAddNewGroup
            // 
            this.buttonAddNewGroup.Location = new System.Drawing.Point(19, 16);
            this.buttonAddNewGroup.Name = "buttonAddNewGroup";
            this.buttonAddNewGroup.Size = new System.Drawing.Size(161, 23);
            this.buttonAddNewGroup.TabIndex = 2;
            this.buttonAddNewGroup.Text = "Add new group";
            this.buttonAddNewGroup.UseVisualStyleBackColor = true;
            this.buttonAddNewGroup.Click += new System.EventHandler(this.buttonAddNewGroup_Click);
            // 
            // buttonRemoveSelected
            // 
            this.buttonRemoveSelected.Location = new System.Drawing.Point(18, 74);
            this.buttonRemoveSelected.Name = "buttonRemoveSelected";
            this.buttonRemoveSelected.Size = new System.Drawing.Size(161, 23);
            this.buttonRemoveSelected.TabIndex = 3;
            this.buttonRemoveSelected.Text = "Remove selected group";
            this.buttonRemoveSelected.UseVisualStyleBackColor = true;
            this.buttonRemoveSelected.Click += new System.EventHandler(this.buttonRemoveSelected_Click);
            // 
            // buttonRenameSelected
            // 
            this.buttonRenameSelected.Location = new System.Drawing.Point(18, 45);
            this.buttonRenameSelected.Name = "buttonRenameSelected";
            this.buttonRenameSelected.Size = new System.Drawing.Size(161, 23);
            this.buttonRenameSelected.TabIndex = 5;
            this.buttonRenameSelected.Text = "Rename selected group";
            this.buttonRenameSelected.UseVisualStyleBackColor = true;
            this.buttonRenameSelected.Click += new System.EventHandler(this.buttonRenaneSelected_Click);
            // 
            // buttonUP
            // 
            this.buttonUP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUP.Location = new System.Drawing.Point(4, 60);
            this.buttonUP.Name = "buttonUP";
            this.buttonUP.Size = new System.Drawing.Size(23, 30);
            this.buttonUP.TabIndex = 6;
            this.buttonUP.Text = "↑";
            this.buttonUP.UseVisualStyleBackColor = true;
            this.buttonUP.Click += new System.EventHandler(this.buttonUP_Click);
            // 
            // buttonDOWN
            // 
            this.buttonDOWN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDOWN.Location = new System.Drawing.Point(4, 96);
            this.buttonDOWN.Name = "buttonDOWN";
            this.buttonDOWN.Size = new System.Drawing.Size(23, 30);
            this.buttonDOWN.TabIndex = 7;
            this.buttonDOWN.Text = "↓";
            this.buttonDOWN.UseVisualStyleBackColor = true;
            this.buttonDOWN.Click += new System.EventHandler(this.buttonDOWN_Click);
            // 
            // groupBoxGroups
            // 
            this.groupBoxGroups.Controls.Add(this.buttonRenameSelected);
            this.groupBoxGroups.Controls.Add(this.buttonAddNewGroup);
            this.groupBoxGroups.Controls.Add(this.buttonRemoveSelected);
            this.groupBoxGroups.Location = new System.Drawing.Point(4, 191);
            this.groupBoxGroups.Name = "groupBoxGroups";
            this.groupBoxGroups.Size = new System.Drawing.Size(191, 103);
            this.groupBoxGroups.TabIndex = 8;
            this.groupBoxGroups.TabStop = false;
            this.groupBoxGroups.Text = "Groups";
            // 
            // groupBoxItems
            // 
            this.groupBoxItems.Controls.Add(this.buttonRemoveItem);
            this.groupBoxItems.Controls.Add(this.buttonAddElement);
            this.groupBoxItems.Location = new System.Drawing.Point(201, 191);
            this.groupBoxItems.Name = "groupBoxItems";
            this.groupBoxItems.Size = new System.Drawing.Size(188, 75);
            this.groupBoxItems.TabIndex = 9;
            this.groupBoxItems.TabStop = false;
            this.groupBoxItems.Text = "Items";
            // 
            // buttonAddElement
            // 
            this.buttonAddElement.Location = new System.Drawing.Point(6, 19);
            this.buttonAddElement.Name = "buttonAddElement";
            this.buttonAddElement.Size = new System.Drawing.Size(161, 23);
            this.buttonAddElement.TabIndex = 6;
            this.buttonAddElement.Text = "Add new vob";
            this.buttonAddElement.UseVisualStyleBackColor = true;
            this.buttonAddElement.Click += new System.EventHandler(this.buttonAddElement_Click);
            // 
            // buttonRemoveItem
            // 
            this.buttonRemoveItem.Location = new System.Drawing.Point(6, 45);
            this.buttonRemoveItem.Name = "buttonRemoveItem";
            this.buttonRemoveItem.Size = new System.Drawing.Size(161, 23);
            this.buttonRemoveItem.TabIndex = 7;
            this.buttonRemoveItem.Text = "Remove selected item";
            this.buttonRemoveItem.UseVisualStyleBackColor = true;
            this.buttonRemoveItem.Click += new System.EventHandler(this.buttonRemoveItem_Click);
            // 
            // buttonUpRight
            // 
            this.buttonUpRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonUpRight.Location = new System.Drawing.Point(546, 60);
            this.buttonUpRight.Name = "buttonUpRight";
            this.buttonUpRight.Size = new System.Drawing.Size(23, 30);
            this.buttonUpRight.TabIndex = 10;
            this.buttonUpRight.Text = "↑";
            this.buttonUpRight.UseVisualStyleBackColor = true;
            this.buttonUpRight.Click += new System.EventHandler(this.buttonUpRight_Click);
            // 
            // buttonDownRight
            // 
            this.buttonDownRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDownRight.Location = new System.Drawing.Point(546, 96);
            this.buttonDownRight.Name = "buttonDownRight";
            this.buttonDownRight.Size = new System.Drawing.Size(23, 30);
            this.buttonDownRight.TabIndex = 11;
            this.buttonDownRight.Text = "↓";
            this.buttonDownRight.UseVisualStyleBackColor = true;
            this.buttonDownRight.Click += new System.EventHandler(this.buttonDownRight_Click);
            // 
            // checkBoxVobCreateActive
            // 
            this.checkBoxVobCreateActive.AutoSize = true;
            this.checkBoxVobCreateActive.Checked = true;
            this.checkBoxVobCreateActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVobCreateActive.Location = new System.Drawing.Point(207, 271);
            this.checkBoxVobCreateActive.Name = "checkBoxVobCreateActive";
            this.checkBoxVobCreateActive.Size = new System.Drawing.Size(215, 17);
            this.checkBoxVobCreateActive.TabIndex = 12;
            this.checkBoxVobCreateActive.Text = "Put selected visual when creating a vob";
            this.checkBoxVobCreateActive.UseVisualStyleBackColor = true;
            // 
            // VobCatalogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 298);
            this.Controls.Add(this.checkBoxVobCreateActive);
            this.Controls.Add(this.buttonDownRight);
            this.Controls.Add(this.buttonUpRight);
            this.Controls.Add(this.groupBoxItems);
            this.Controls.Add(this.groupBoxGroups);
            this.Controls.Add(this.buttonDOWN);
            this.Controls.Add(this.buttonUP);
            this.Controls.Add(this.listBoxItems);
            this.Controls.Add(this.listBoxGroups);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VobCatalogForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VobCatalog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VobCatalogForm_FormClosing);
            this.Shown += new System.EventHandler(this.VobCatalogForm_Shown);
            this.VisibleChanged += new System.EventHandler(this.VobCatalogForm_VisibleChanged);
            this.groupBoxGroups.ResumeLayout(false);
            this.groupBoxItems.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxGroups;
        private System.Windows.Forms.Button buttonAddNewGroup;
        private System.Windows.Forms.Button buttonRemoveSelected;
        private System.Windows.Forms.Button buttonRenameSelected;
        private System.Windows.Forms.Button buttonUP;
        private System.Windows.Forms.Button buttonDOWN;
        private System.Windows.Forms.GroupBox groupBoxGroups;
        private System.Windows.Forms.GroupBox groupBoxItems;
        private System.Windows.Forms.Button buttonAddElement;
        private System.Windows.Forms.Button buttonRemoveItem;
        private System.Windows.Forms.Button buttonUpRight;
        private System.Windows.Forms.Button buttonDownRight;
        public System.Windows.Forms.CheckBox checkBoxVobCreateActive;
        public System.Windows.Forms.ListBox listBoxItems;
    }
}