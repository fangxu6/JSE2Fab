

namespace DataToExcel
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using Jcap.TI.Convert;

    public partial class frmTIView : Form
    {
        private IMapingFile _currFile;

        // 构造函数
        public frmTIView()
        {
            InitializeComponent();
        }

        // 窗体登陆
        private void frmTIView_Load(object sender, EventArgs e)
        {
            try
            {
                // 加载下拉框选项
                this.cmbFileType.Items.Add("Tiww");
                this.cmbFileType.Items.Add("Sinf");
                this.cmbFileType.Items.Add("Tma");
                this.cmbFileType.Items.Add("Tsk");
                this.cmbFileType.SelectedIndex = 0;

                this.cmbShowScale.Items.Add("Auto");
                this.cmbShowScale.Items.Add("100%");
                this.cmbShowScale.Items.Add("200%");
                this.cmbShowScale.Items.Add("300%");
                this.cmbShowScale.Items.Add("400%");
                this.cmbShowScale.Items.Add("500%");
                this.cmbShowScale.SelectedIndex = 0;

                this.cmbPrintScale.Items.Add("100%");
                this.cmbPrintScale.Items.Add("Screen");
                this.cmbPrintScale.SelectedIndex = 0;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        // 加载 tiww 格式 mapping 文件
        private void menuLoad_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadMappingFile();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        // 加载 mapping file
        private void LoadMappingFile()
        {
            switch (this.cmbFileType.SelectedIndex)
            {

                // sinf
                case 1:
                    this.LoadSinf();
                    break;

                // tsk
                case 3:
                    this.LoadTsk();
                    break;
            }
        }



        private void LoadSinf()
        {
            OpenFileDialog od = new OpenFileDialog();
            od.RestoreDirectory = false;
            od.Filter = "sinf mapping file(*.sinf)|*.sinf";
            od.Multiselect = true;

            if (od.ShowDialog() == DialogResult.OK)
            {
                this.lsvItems.Items.Clear();

                foreach (string f in od.FileNames)
                {
                    Sinf sinf = new Sinf(f);
                    sinf.Read();

                    ListViewItem lvi = new ListViewItem(sinf.Wafer);
                    lvi.Tag = sinf;

                    this.lsvItems.Items.Add(lvi);
                }
            }
        }



        private void LoadTsk()
        {
            OpenFileDialog od = new OpenFileDialog();
            od.RestoreDirectory = false;
            od.Multiselect = true;

            if (od.ShowDialog() == DialogResult.OK)
            {
                this.lsvItems.Items.Clear();

                foreach (string f in od.FileNames)
                {
                    Tsk tsk = new Tsk(f);
                    tsk.Read();

                    ListViewItem lvi = new ListViewItem(tsk.WaferID);
                    lvi.Tag = tsk;

                    this.lsvItems.Items.Add(lvi);
                }
            }
        }

        // 关闭文件
        private void menuClose_Click(object sender, EventArgs e)
        {
            this._currFile = null;

            this.lsvItems.Columns[0].Text = "mapping file";
            this.lsvItems.Items.Clear();
        }

        // 双击列表查看详细 mapping 信息
        private void lsvItems_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                switch (this.cmbFileType.SelectedIndex)
                {

                        // sinf
                    case 1:
                        this.ShowSinf();
                        break;

                        // tsk
                    case 3:
                        this.ShowTsk();
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void ShowTiww()
        {

        }

        private void ShowSinf()
        {
            if (this.lsvItems.SelectedItems.Count <= 0)
                return;

            this._currFile = (IMapingFile)this.lsvItems.SelectedItems[0].Tag;

            string msg = "\n";

            msg += "           Lot：" + ((Sinf)this._currFile).Lot + "\n";
            msg += "       WaferNo：" + ((Sinf)this._currFile).Wafer + "\n";
            msg += "        Device：" + ((Sinf)this._currFile).Device + "\n";
            msg += "\n";
            msg += "         Refpx：" + ((Sinf)this._currFile).Refpx + "\n";
            msg += "         Refpy：" + ((Sinf)this._currFile).Refpy + "\n";
            msg += "\n";

            msg += "          Cols：" + ((Sinf)this._currFile).ColCount + "\n";
            msg += "          Rows：" + ((Sinf)this._currFile).RowCount + "\n";
            msg += "\n";
            msg += "    Total dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.FailDie | DieCategory.MarkDie | DieCategory.PassDie
                | DieCategory.SkipDie | DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow).ToString() + "\n";
            msg += "     Pass dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.PassDie | DieCategory.TIRefPass).ToString() + "\n";
            msg += "     Fail dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.FailDie | DieCategory.TIRefFail).ToString() + "\n";
            msg += "     Mark dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.MarkDie).ToString() + "\n";
            msg += "  SkipDie dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.SkipDie).ToString() + "\n";
            msg += "   Unknow dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.Unknow).ToString() + "\n";

            this.txtInfo.Text = msg;

            this.pnlMapping.Refresh();
        }


        private void ShowTsk()
        {
            if (this.lsvItems.SelectedItems.Count <= 0)
                return;

            this._currFile = (IMapingFile)this.lsvItems.SelectedItems[0].Tag;

            string msg = "\n";

            msg += "           Lot：" + ((Tsk)this._currFile).LotNo + "\n";
            msg += "       WaferID：" + ((Tsk)this._currFile).WaferID + "\n";
            msg += "        Device：" + ((Tsk)this._currFile).Device + "\n";
            msg += "\n";
            msg += "         Refpx：" + ((Tsk)this._currFile).Refpx + "\n";
            msg += "         Refpy：" + ((Tsk)this._currFile).Refpy + "\n";
            msg += "\n";
            msg += "    Wafer size：" + ((Tsk)this._currFile).WaferSize + "\n";
            msg += "\n";

            msg += "          Cols：" + this._currFile.DieMatrix.XMax.ToString() + "\n";
            msg += "          Rows：" + this._currFile.DieMatrix.YMax.ToString() + "\n";
            msg += "\n";
            msg += "    Total dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.FailDie | DieCategory.MarkDie | DieCategory.PassDie
                | DieCategory.SkipDie | DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow).ToString() + "\n";
            msg += "     Pass dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.PassDie | DieCategory.TIRefPass).ToString() + "\n";
            msg += "     Fail dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.FailDie | DieCategory.TIRefFail).ToString() + "\n";
            msg += "     Mark dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.MarkDie).ToString() + "\n";
            msg += "  SkipDie dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.SkipDie).ToString() + "\n";
            msg += "   Unknow dies：" + this._currFile.DieMatrix.DieAttributeStat(DieCategory.Unknow).ToString() + "\n";

            this.txtInfo.Text = msg;

            this.pnlMapping.Refresh();
        }

        // 重绘 mapping 图
        private void pnlMapping_Paint(object sender, PaintEventArgs e)
        {
            this.Draw(e.Graphics);
        }

        // 改变尺寸后重绘
        private void pnlMapping_Resize(object sender, EventArgs e)
        {
            this.Draw(Graphics.FromHwnd(this.pnlMapping.Handle));
        }

        // 在绘图区内绘制 mapping 图
        private void Draw(Graphics g)
        {
            if (this._currFile == null)
                g.FillRectangle(SystemBrushes.Window, 0, 0, (float)this.pnlMapping.Width, (float)this.pnlMapping.Height);
            else
                this.DrawMatrix(g);
        }

        // 绘制 mapping 矩阵
        private void DrawMatrix(Graphics g)
        {
            switch (this.cmbFileType.SelectedIndex)
            {
                    // tiww
                case 0:
                    this._currFile.DieMatrix.Paint(g, 
                        (float)((decimal)this._currFile.Properties["X_SIZE"]),
                        (float)((decimal)this._currFile.Properties["Y_SIZE"]),  false);
                    break;
                    // sinf、tma、tsk
                case 1:
                case 2:
                case 3:
                    this._currFile.DieMatrix.Paint(g, this.pnlMapping.Bounds,  false);
                    break;
            }
        }



        private void printer_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        private void cmbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}