using System;
using System.ComponentModel;
using ToeOSCCore;

namespace ToesOSC {
    public partial class ToeManagerWindow : Form {
        private ToeManager _toeManager;

        private List<Button> _buttonsLeft = new List<Button>();
        private List<Button> _buttonsRight = new List<Button>();
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

        public List<Button> ButtonsLeft { get => _buttonsLeft; set => _buttonsLeft = value; }
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

        public List<Button> ButtonsRight { get => _buttonsRight; set => _buttonsRight = value; }

        public ToeManagerWindow() {
            InitializeComponent();
            _toeManager = new ToeManager();
            _buttonsLeft = new List<Button> { leftToeButton1, leftToeButton2, leftToeButton3, leftToeButton4 };
            _buttonsRight = new List<Button> { rightToeButton1, rightToeButton2, rightToeButton3, rightToeButton4 };

            for (int i = 0; i < _buttonsLeft.Count; i++) {
                int index = i + 1;
                _buttonsLeft[i].Click += delegate {
                    ToggleToe(index, ToeManager.FootSide.Left);
                };
            }
            for (int i = 0; i < _buttonsRight.Count; i++) {
                int index = i + 1;
                _buttonsRight[i].Click += delegate {
                    ToggleToe(index, ToeManager.FootSide.Right);
                };
            }
            foreach(Control control in Controls) {
                control.KeyDown += ToeManagerWindow_KeyDown;
                control.KeyUp += ToeManagerWindow_KeyUp;
            }
        }
        public void ToggleToe(int index, ToeManager.FootSide footSide) {
            bool currentToeValue = _toeManager.GetToeValue(index, footSide);
            _toeManager.SetToeValue(index, footSide, !currentToeValue);
        }


        private void pictureBox1_Click(object sender, EventArgs e) {

        }

        private void bendAllToesRight_Click(object sender, EventArgs e) {
            for (int i = 0; i < _buttonsRight.Count; i++) {
                ToggleToe(i + 1, ToeManager.FootSide.Right);
            }
        }

        private void button2_Click(object sender, EventArgs e) {

        }

        private void rightToeButton4_Click(object sender, EventArgs e) {

        }

        private void bendSmallToesButtonLeft_Click(object sender, EventArgs e) {
            for (int i = 1; i < _buttonsLeft.Count; i++) {
                ToggleToe(i + 1, ToeManager.FootSide.Left);
            }
        }

        private void bendAllToesLeft_Click(object sender, EventArgs e) {
            for (int i = 0; i < _buttonsLeft.Count; i++) {
                ToggleToe(i + 1, ToeManager.FootSide.Left);
            }
        }

        private void ToeManagerWindow_KeyDown(object sender, KeyEventArgs e) {
                switch (e.KeyCode) {
                case Keys.Q:
                    _toeManager.SetToeValue(1, ToeManager.FootSide.Left, true);
                    break;
                case Keys.W:
                    _toeManager.SetToeValue(2, ToeManager.FootSide.Left, true);
                    break;
                case Keys.E:
                    _toeManager.SetToeValue(3, ToeManager.FootSide.Left, true);
                    break;
                case Keys.R:
                    _toeManager.SetToeValue(4, ToeManager.FootSide.Left, true);
                    break;

                case Keys.A:
                    _toeManager.SetToeValue(1, ToeManager.FootSide.Right, true);
                    break;
                case Keys.S:
                    _toeManager.SetToeValue(2, ToeManager.FootSide.Right, true);
                    break;
                case Keys.D:
                    _toeManager.SetToeValue(3, ToeManager.FootSide.Right, true);
                    break;
                case Keys.F:
                    _toeManager.SetToeValue(4, ToeManager.FootSide.Right, true);
                    break;
            }
        }

        private void ToeManagerWindow_KeyPress(object sender, KeyPressEventArgs e) {

        }

        private void ToeManagerWindow_KeyUp(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Q:
                    _toeManager.SetToeValue(1, ToeManager.FootSide.Left, false);
                    break;
                case Keys.W:
                    _toeManager.SetToeValue(2, ToeManager.FootSide.Left, false);
                    break;
                case Keys.E:
                    _toeManager.SetToeValue(3, ToeManager.FootSide.Left, false);
                    break;
                case Keys.R:
                    _toeManager.SetToeValue(4, ToeManager.FootSide.Left, false);
                    break;

                case Keys.A:
                    _toeManager.SetToeValue(1, ToeManager.FootSide.Right, false);
                    break;
                case Keys.S:
                    _toeManager.SetToeValue(2, ToeManager.FootSide.Right, false);
                    break;
                case Keys.D:
                    _toeManager.SetToeValue(3, ToeManager.FootSide.Right, false);
                    break;
                case Keys.F:
                    _toeManager.SetToeValue(4, ToeManager.FootSide.Right, false);
                    break;
            }
        }

        private void bendSmallToesButtonRight_Click(object sender, EventArgs e) {
            for (int i = 1; i < _buttonsRight.Count; i++) {
                ToggleToe(i + 1, ToeManager.FootSide.Right);
            }
        }
    }
}
