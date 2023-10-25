using System.Diagnostics;

namespace ACgifts;
partial class DetailForm:Form
{
	private Delegate? refreshMethod;
	private Neighbor? neighbor;
	private bool shutdown = false;
	private readonly string dtpFormat = "dd MMM yyyy  HH:mmtt";

	public DetailForm()
	{
		InitializeComponent();
		Visible = false;
	}


	public void UpdateData(Neighbor n, Point openAt, Delegate refreshMethod)
	{
		this.refreshMethod = refreshMethod;
		this.neighbor = n;
		Left = openAt.X;
		Top = openAt.Y;
		Visible = true;
		Text = $"{n.Name} details";

		tbName.Text = n.Name;
		tbGroup.Text = n.Group;
		tbRecvCode.Text = n.IdRecv;
		tbSendCode.Text = n.IdSend;
		tbSendName.Text = n.NameSend;
		tbRecvName.Text = n.NameRecv;
		dtpAdded.Value = n.Added;

		dtpAdded.MaxDate = DateTime.Now < n.Added ? n.Added : DateTime.Now;
		dtpLastRecv.MaxDate = DateTime.Now;
		dtpLastSend.MaxDate = DateTime.Now;


		if(n.LastRecv is null)
		{
			dtpLastRecv.Value = dtpLastRecv.MinDate;
			dtpLastRecv.CustomFormat = " ";
		}
		else
		{
			dtpLastRecv.CustomFormat = dtpFormat;
			dtpLastRecv.Value = (DateTime)n.LastRecv;
		}


		if(n.LastSend is null)
		{
			dtpLastSend.Value = dtpLastSend.MinDate;
			dtpLastSend.CustomFormat = " ";
		}
		else
		{
			dtpLastSend.CustomFormat = dtpFormat;
			dtpLastSend.Value = (DateTime)n.LastSend;
		}

	}

	public void CloseForm()
	{
		shutdown = true;
		Close();
	}

	private void DetailForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		Debug.WriteLine(e.CloseReason);
		e.Cancel = !shutdown;
		Visible = false;
	}



	private void Dtp_DropDown(object sender, EventArgs e)
	{
		if(sender is not DateTimePicker dtp) return;

		if(dtp.Value == dtp.MinDate)
		{
			dtp.Value = dtp.MaxDate;
			dtp.CustomFormat = dtpFormat;
			dtp.Focus();
			SendKeys.Send("{ESC}");
			SendKeys.Send("%{DOWN}");
		}
	}
	private void Dtp_MouseUp(object sender, MouseEventArgs e)
	{
		if(sender is not DateTimePicker dtp) return;

		if(e.Button == MouseButtons.Right)
		{
			ContextMenuStrip cm = new();
			cm.Items.Add(new ToolStripMenuItem($"Remove date", null, CtxSetDateNull) { Tag = dtp });
			Utils.ShowContextOnScreen(this, cm, dtp.PointToScreen(e.Location));
		}
	}
	private void CtxSetDateNull(object? sender, EventArgs? e)
	{
		if(sender is not ToolStripMenuItem tsmi) return;
		if(tsmi.Tag is not DateTimePicker dtp) return;
		dtp.Value = dtp.MinDate;
		dtp.CustomFormat = " ";
	}

	private void ButClose_Click(object sender, EventArgs e)
	{
		Visible = false;
	}

	private void ButSave_Click(object sender, EventArgs e)
	{
		if(neighbor != null)
		{
			neighbor.Name = tbName.Text;
			neighbor.Group = tbGroup.Text;
			neighbor.IdRecv = tbRecvCode.Text;
			neighbor.IdSend = tbSendCode.Text;
			neighbor.NameSend = tbSendName.Text;
			neighbor.NameRecv = tbRecvName.Text;
			neighbor.Added = dtpAdded.Value;
			neighbor.LastRecv = dtpLastRecv.Value == dtpLastRecv.MinDate ? null : dtpLastRecv.Value;
			neighbor.LastSend = dtpLastSend.Value == dtpLastSend.MinDate ? null : dtpLastSend.Value;
		}
		refreshMethod?.DynamicInvoke();
		Visible = false;
	}
}
