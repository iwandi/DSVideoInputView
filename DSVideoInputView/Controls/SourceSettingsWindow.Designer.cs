
namespace DSVideoInputView
{
    partial class SourceSettingsWindow
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.radioButtonLetterBox = new System.Windows.Forms.RadioButton();
            this.radioButtonStrech = new System.Windows.Forms.RadioButton();
            this.groupBoxFit = new System.Windows.Forms.GroupBox();
            this.groupBoxResolution = new System.Windows.Forms.GroupBox();
            this.labelWidthMulti = new System.Windows.Forms.Label();
            this.labelHeightMulti = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.textHeight = new System.Windows.Forms.TextBox();
            this.textHeightMulti = new System.Windows.Forms.TextBox();
            this.textWidthMulti = new System.Windows.Forms.TextBox();
            this.textWidth = new System.Windows.Forms.TextBox();
            this.listBoxSource = new System.Windows.Forms.ListBox();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.radioButtonOutputBased = new System.Windows.Forms.RadioButton();
            this.radioButtonSourceBased = new System.Windows.Forms.RadioButton();
            this.groupBoxSource = new System.Windows.Forms.GroupBox();
            this.labelSourceDisplayName = new System.Windows.Forms.Label();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.labelSourceName = new System.Windows.Forms.Label();
            this.groupBoxFit.SuspendLayout();
            this.groupBoxResolution.SuspendLayout();
            this.groupBoxSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(310, 660);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.CancelClick);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(472, 660);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.OkClick);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(391, 660);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.ApplyClick);
            // 
            // radioButtonLetterBox
            // 
            this.radioButtonLetterBox.AutoSize = true;
            this.radioButtonLetterBox.Location = new System.Drawing.Point(6, 19);
            this.radioButtonLetterBox.Name = "radioButtonLetterBox";
            this.radioButtonLetterBox.Size = new System.Drawing.Size(73, 17);
            this.radioButtonLetterBox.TabIndex = 3;
            this.radioButtonLetterBox.TabStop = true;
            this.radioButtonLetterBox.Text = "Letter Box";
            this.radioButtonLetterBox.UseVisualStyleBackColor = true;
            // 
            // radioButtonStrech
            // 
            this.radioButtonStrech.AutoSize = true;
            this.radioButtonStrech.Location = new System.Drawing.Point(6, 42);
            this.radioButtonStrech.Name = "radioButtonStrech";
            this.radioButtonStrech.Size = new System.Drawing.Size(56, 17);
            this.radioButtonStrech.TabIndex = 4;
            this.radioButtonStrech.TabStop = true;
            this.radioButtonStrech.Text = "Strech";
            this.radioButtonStrech.UseVisualStyleBackColor = true;
            // 
            // groupBoxFit
            // 
            this.groupBoxFit.Controls.Add(this.radioButtonStrech);
            this.groupBoxFit.Controls.Add(this.radioButtonLetterBox);
            this.groupBoxFit.Location = new System.Drawing.Point(12, 12);
            this.groupBoxFit.Name = "groupBoxFit";
            this.groupBoxFit.Size = new System.Drawing.Size(185, 69);
            this.groupBoxFit.TabIndex = 5;
            this.groupBoxFit.TabStop = false;
            this.groupBoxFit.Text = "Source Aspect Fit";
            // 
            // groupBoxResolution
            // 
            this.groupBoxResolution.Controls.Add(this.labelWidthMulti);
            this.groupBoxResolution.Controls.Add(this.labelHeightMulti);
            this.groupBoxResolution.Controls.Add(this.labelHeight);
            this.groupBoxResolution.Controls.Add(this.labelWidth);
            this.groupBoxResolution.Controls.Add(this.textHeight);
            this.groupBoxResolution.Controls.Add(this.textHeightMulti);
            this.groupBoxResolution.Controls.Add(this.textWidthMulti);
            this.groupBoxResolution.Controls.Add(this.textWidth);
            this.groupBoxResolution.Controls.Add(this.listBoxSource);
            this.groupBoxResolution.Controls.Add(this.listBoxOutput);
            this.groupBoxResolution.Controls.Add(this.radioButtonOutputBased);
            this.groupBoxResolution.Controls.Add(this.radioButtonSourceBased);
            this.groupBoxResolution.Location = new System.Drawing.Point(12, 87);
            this.groupBoxResolution.Name = "groupBoxResolution";
            this.groupBoxResolution.Size = new System.Drawing.Size(532, 352);
            this.groupBoxResolution.TabIndex = 6;
            this.groupBoxResolution.TabStop = false;
            this.groupBoxResolution.Text = "Resolution";
            // 
            // labelWidthMulti
            // 
            this.labelWidthMulti.AutoSize = true;
            this.labelWidthMulti.Location = new System.Drawing.Point(6, 43);
            this.labelWidthMulti.Name = "labelWidthMulti";
            this.labelWidthMulti.Size = new System.Drawing.Size(76, 13);
            this.labelWidthMulti.TabIndex = 11;
            this.labelWidthMulti.Text = "With Multiplyer";
            // 
            // labelHeightMulti
            // 
            this.labelHeightMulti.AutoSize = true;
            this.labelHeightMulti.Location = new System.Drawing.Point(6, 69);
            this.labelHeightMulti.Name = "labelHeightMulti";
            this.labelHeightMulti.Size = new System.Drawing.Size(85, 13);
            this.labelHeightMulti.TabIndex = 10;
            this.labelHeightMulti.Text = "Height Multiplyer";
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(269, 69);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(38, 13);
            this.labelHeight.TabIndex = 9;
            this.labelHeight.Text = "Height";
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(269, 43);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(35, 13);
            this.labelWidth.TabIndex = 8;
            this.labelWidth.Text = "Width";
            // 
            // textHeight
            // 
            this.textHeight.Location = new System.Drawing.Point(396, 66);
            this.textHeight.Name = "textHeight";
            this.textHeight.Size = new System.Drawing.Size(128, 20);
            this.textHeight.TabIndex = 7;
            // 
            // textHeightMulti
            // 
            this.textHeightMulti.Location = new System.Drawing.Point(135, 66);
            this.textHeightMulti.Name = "textHeightMulti";
            this.textHeightMulti.Size = new System.Drawing.Size(128, 20);
            this.textHeightMulti.TabIndex = 6;
            // 
            // textWidthMulti
            // 
            this.textWidthMulti.Location = new System.Drawing.Point(136, 40);
            this.textWidthMulti.Name = "textWidthMulti";
            this.textWidthMulti.Size = new System.Drawing.Size(128, 20);
            this.textWidthMulti.TabIndex = 5;
            // 
            // textWidth
            // 
            this.textWidth.Location = new System.Drawing.Point(396, 40);
            this.textWidth.Name = "textWidth";
            this.textWidth.Size = new System.Drawing.Size(128, 20);
            this.textWidth.TabIndex = 4;
            // 
            // listBoxSource
            // 
            this.listBoxSource.FormattingEnabled = true;
            this.listBoxSource.Location = new System.Drawing.Point(10, 94);
            this.listBoxSource.Name = "listBoxSource";
            this.listBoxSource.Size = new System.Drawing.Size(254, 251);
            this.listBoxSource.TabIndex = 3;
            this.listBoxSource.SelectedIndexChanged += new System.EventHandler(this.SourceSelectionChnaged);
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.Location = new System.Drawing.Point(270, 94);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(254, 251);
            this.listBoxOutput.TabIndex = 2;
            this.listBoxOutput.SelectedIndexChanged += new System.EventHandler(this.OutputSelectionChnaged);
            // 
            // radioButtonOutputBased
            // 
            this.radioButtonOutputBased.AutoSize = true;
            this.radioButtonOutputBased.Location = new System.Drawing.Point(272, 19);
            this.radioButtonOutputBased.Name = "radioButtonOutputBased";
            this.radioButtonOutputBased.Size = new System.Drawing.Size(90, 17);
            this.radioButtonOutputBased.TabIndex = 1;
            this.radioButtonOutputBased.TabStop = true;
            this.radioButtonOutputBased.Text = "Output Based";
            this.radioButtonOutputBased.UseVisualStyleBackColor = true;
            this.radioButtonOutputBased.CheckedChanged += new System.EventHandler(this.ResolutionSourceCheckedChanged);
            // 
            // radioButtonSourceBased
            // 
            this.radioButtonSourceBased.AutoSize = true;
            this.radioButtonSourceBased.Location = new System.Drawing.Point(6, 19);
            this.radioButtonSourceBased.Name = "radioButtonSourceBased";
            this.radioButtonSourceBased.Size = new System.Drawing.Size(92, 17);
            this.radioButtonSourceBased.TabIndex = 0;
            this.radioButtonSourceBased.TabStop = true;
            this.radioButtonSourceBased.Text = "Source Based";
            this.radioButtonSourceBased.UseVisualStyleBackColor = true;
            this.radioButtonSourceBased.CheckedChanged += new System.EventHandler(this.ResolutionSourceCheckedChanged);
            // 
            // groupBoxSource
            // 
            this.groupBoxSource.Controls.Add(this.labelSourceDisplayName);
            this.groupBoxSource.Controls.Add(this.buttonConfig);
            this.groupBoxSource.Controls.Add(this.labelSourceName);
            this.groupBoxSource.Location = new System.Drawing.Point(203, 12);
            this.groupBoxSource.Name = "groupBoxSource";
            this.groupBoxSource.Size = new System.Drawing.Size(341, 69);
            this.groupBoxSource.TabIndex = 7;
            this.groupBoxSource.TabStop = false;
            this.groupBoxSource.Text = "Source";
            // 
            // labelSourceDisplayName
            // 
            this.labelSourceDisplayName.AutoSize = true;
            this.labelSourceDisplayName.Location = new System.Drawing.Point(6, 41);
            this.labelSourceDisplayName.Name = "labelSourceDisplayName";
            this.labelSourceDisplayName.Size = new System.Drawing.Size(79, 13);
            this.labelSourceDisplayName.TabIndex = 3;
            this.labelSourceDisplayName.Text = "<displayName>";
            // 
            // buttonConfig
            // 
            this.buttonConfig.Location = new System.Drawing.Point(260, 40);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonConfig.TabIndex = 2;
            this.buttonConfig.Text = "Config";
            this.buttonConfig.UseVisualStyleBackColor = true;
            this.buttonConfig.Click += new System.EventHandler(this.ConfigClick);
            // 
            // labelSourceName
            // 
            this.labelSourceName.AutoSize = true;
            this.labelSourceName.Location = new System.Drawing.Point(6, 21);
            this.labelSourceName.Name = "labelSourceName";
            this.labelSourceName.Size = new System.Drawing.Size(45, 13);
            this.labelSourceName.TabIndex = 0;
            this.labelSourceName.Text = "<name>";
            // 
            // SourceSettingsWindow
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(559, 695);
            this.Controls.Add(this.groupBoxSource);
            this.Controls.Add(this.groupBoxResolution);
            this.Controls.Add(this.groupBoxFit);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SourceSettingsWindow";
            this.ShowInTaskbar = false;
            this.Text = "Source Settings";
            this.groupBoxFit.ResumeLayout(false);
            this.groupBoxFit.PerformLayout();
            this.groupBoxResolution.ResumeLayout(false);
            this.groupBoxResolution.PerformLayout();
            this.groupBoxSource.ResumeLayout(false);
            this.groupBoxSource.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.RadioButton radioButtonLetterBox;
        private System.Windows.Forms.RadioButton radioButtonStrech;
        private System.Windows.Forms.GroupBox groupBoxFit;
        private System.Windows.Forms.GroupBox groupBoxResolution;
        private System.Windows.Forms.Label labelWidthMulti;
        private System.Windows.Forms.Label labelHeightMulti;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.TextBox textHeight;
        private System.Windows.Forms.TextBox textHeightMulti;
        private System.Windows.Forms.TextBox textWidthMulti;
        private System.Windows.Forms.TextBox textWidth;
        private System.Windows.Forms.ListBox listBoxSource;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.RadioButton radioButtonOutputBased;
        private System.Windows.Forms.RadioButton radioButtonSourceBased;
        private System.Windows.Forms.GroupBox groupBoxSource;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Label labelSourceName;
        private System.Windows.Forms.Label labelSourceDisplayName;
    }
}