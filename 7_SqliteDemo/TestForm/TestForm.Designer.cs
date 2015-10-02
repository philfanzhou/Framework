namespace Entity_Framework
{
    partial class TestForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_GetAllRecords = new System.Windows.Forms.Button();
            this.button_AddRecord = new System.Windows.Forms.Button();
            this.button_delRecord = new System.Windows.Forms.Button();
            this.button_EditRecord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_GetAllRecords
            // 
            this.button_GetAllRecords.Location = new System.Drawing.Point(12, 126);
            this.button_GetAllRecords.Name = "button_GetAllRecords";
            this.button_GetAllRecords.Size = new System.Drawing.Size(124, 28);
            this.button_GetAllRecords.TabIndex = 0;
            this.button_GetAllRecords.Text = "Get all records";
            this.button_GetAllRecords.UseVisualStyleBackColor = true;
            this.button_GetAllRecords.Click += new System.EventHandler(this.button_GetAllRecords_Click);
            // 
            // button_AddRecord
            // 
            this.button_AddRecord.Location = new System.Drawing.Point(12, 25);
            this.button_AddRecord.Name = "button_AddRecord";
            this.button_AddRecord.Size = new System.Drawing.Size(124, 27);
            this.button_AddRecord.TabIndex = 1;
            this.button_AddRecord.Text = "Add a record";
            this.button_AddRecord.UseVisualStyleBackColor = true;
            this.button_AddRecord.Click += new System.EventHandler(this.button_AddRecord_Click);
            // 
            // button_delRecord
            // 
            this.button_delRecord.Location = new System.Drawing.Point(12, 58);
            this.button_delRecord.Name = "button_delRecord";
            this.button_delRecord.Size = new System.Drawing.Size(124, 28);
            this.button_delRecord.TabIndex = 2;
            this.button_delRecord.Text = "Delete a record";
            this.button_delRecord.UseVisualStyleBackColor = true;
            this.button_delRecord.Click += new System.EventHandler(this.button_DelRecord_Click);
            // 
            // button_EditRecord
            // 
            this.button_EditRecord.Location = new System.Drawing.Point(12, 92);
            this.button_EditRecord.Name = "button_EditRecord";
            this.button_EditRecord.Size = new System.Drawing.Size(124, 28);
            this.button_EditRecord.TabIndex = 3;
            this.button_EditRecord.Text = "Edit a record";
            this.button_EditRecord.UseVisualStyleBackColor = true;
            this.button_EditRecord.Click += new System.EventHandler(this.button_EditRecord_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(146, 174);
            this.Controls.Add(this.button_EditRecord);
            this.Controls.Add(this.button_delRecord);
            this.Controls.Add(this.button_AddRecord);
            this.Controls.Add(this.button_GetAllRecords);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Application";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_GetAllRecords;
        private System.Windows.Forms.Button button_AddRecord;
        private System.Windows.Forms.Button button_delRecord;
        private System.Windows.Forms.Button button_EditRecord;
    }
}

