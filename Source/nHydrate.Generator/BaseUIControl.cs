namespace nHydrate.Generator.Common.GeneratorFramework
{
  public class BaseUIControl : ModelObjectUserInterface
  {
    protected System.Windows.Forms.Button mBtnCancel;
    protected System.Windows.Forms.Button mBtnApply;
    private System.Windows.Forms.Panel panel1;
    private readonly System.ComponentModel.Container components = null;

    public BaseUIControl()
    {
      InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code
    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.mBtnCancel = new System.Windows.Forms.Button();
      this.mBtnApply = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // mBtnCancel
      // 
      this.mBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mBtnCancel.Enabled = false;
      this.mBtnCancel.Location = new System.Drawing.Point(96, 4);
      this.mBtnCancel.Name = "mBtnCancel";
      this.mBtnCancel.TabIndex = 9;
      this.mBtnCancel.Text = "&Cancel";
      this.mBtnCancel.Click += new System.EventHandler(this.mBtnCancel_Click);
      // 
      // mBtnApply
      // 
      this.mBtnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mBtnApply.Enabled = false;
      this.mBtnApply.Location = new System.Drawing.Point(8, 4);
      this.mBtnApply.Name = "mBtnApply";
      this.mBtnApply.TabIndex = 8;
      this.mBtnApply.Text = "&Apply";
      this.mBtnApply.Click += new System.EventHandler(this.mBtnApply_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.mBtnApply);
      this.panel1.Controls.Add(this.mBtnCancel);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(176, 32);
      this.panel1.TabIndex = 10;
      // 
      // BaseUIControl
      // 
      this.Controls.Add(this.panel1);
      this.Name = "BaseUIControl";
      this.Size = new System.Drawing.Size(176, 32);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion

    #region Events

    public event StandardEventHandler ApplyButtonClick;
    public event StandardEventHandler CancelButtonClick;

    protected void OnApplyButtonClick(object sender, System.EventArgs e)
    {
      if (this.ApplyButtonClick != null)
        this.ApplyButtonClick(sender, e);
    }

    protected void OnCancelButtonClick(object sender, System.EventArgs e)
    {
      if (this.CancelButtonClick != null)
        this.CancelButtonClick(sender, e);
    }

    #endregion

    #region Button Handlers

    private void mBtnApply_Click(object sender, System.EventArgs e)
    {
      this.OnApplyButtonClick(this, e);
    }

    private void mBtnCancel_Click(object sender, System.EventArgs e)
    {
      this.OnCancelButtonClick(this, e);
    }

    #endregion

  }
}
