unit UPrincipal;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms,
  Dialogs, ACBrBase, ACBrDevice, ACBrETQ, StdCtrls, ExtDlgs, ExtCtrls;

type
  TFPrincipal = class(TForm)
    ACBrETQ: TACBrETQ;
    cbModelo: TComboBox;
    Label1: TLabel;
    Label2: TLabel;
    cbPorta: TComboBox;
    eCopias: TEdit;
    Label3: TLabel;
    eAvanco: TEdit;
    Label4: TLabel;
    bEtqSimples: TButton;
    bEtqCarreiras: TButton;
    Image1: TImage;
    Edit1: TEdit;
    Label5: TLabel;
    Button2: TButton;
    Button1: TButton;
    OpenPictureDialog1: TOpenPictureDialog;
    ckMemoria: TCheckBox;
    procedure bEtqSimplesClick(Sender: TObject);
    procedure bEtqCarreirasClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  FPrincipal: TFPrincipal;

implementation

{$R *.dfm}

procedure TFPrincipal.FormCreate(Sender: TObject);
begin
  cbModelo.ItemIndex := 1 ;
end;

procedure TFPrincipal.bEtqSimplesClick(Sender: TObject);
begin
  with ACBrETQ do
  begin
     Modelo := TACBrETQModelo(cbModelo.ItemIndex);
     Porta := cbPorta.Text;
     LimparMemoria := ckMemoria.Checked ;
     Ativar;

     ImprimirTexto(orNormal, 2, 2, 2, 190, 5, 'BISCOITO MARILAN RECH 335G');
     ImprimirTexto(orNormal, 2, 2, 1, 158, 5, 'CHOC BRANCO');
     ImprimirBarras(orNormal, 'F', '2', '2', 32, 0, '7896003701685', 90);
     ImprimirTexto(orNormal, 3, 3, 2, 15, 300, 'R$');
     ImprimirTexto(orNormal, 3, 4, 4, 15, 450, '20.59');

     Imprimir(StrToInt(eCopias.Text), StrToInt(eAvanco.Text));
     Desativar;
  end;
end;

procedure TFPrincipal.bEtqCarreirasClick(Sender: TObject);
begin
  with ACBrETQ do
  begin
     Modelo := TACBrETQModelo(cbModelo.ItemIndex);
     Porta := cbPorta.Text;
     Avanco := StrToInt(eAvanco.Text);
     LimparMemoria := ckMemoria.Checked ;
     Ativar;

     ImprimirTexto(orNormal, 2, 1, 2, 0180, 0015, 'BISCOITO REC 335G');
     ImprimirTexto(orNormal, 2, 1, 1, 0140, 0015, 'CHOC BRANCO');
     ImprimirBarras(orNormal, 'F', '2', '2', 0020, 0010, '7896003701685', 070);

     ImprimirTexto(orNormal, 2, 1, 2, 0180, 0315, 'BISCOITO RECH 335G');
     ImprimirTexto(orNormal, 2, 1, 1, 0140, 0315, 'CHOC BRANCO');
     ImprimirBarras(orNormal, 'F', '2', '2', 0020, 0315, '7896003701685', 070);

     ImprimirTexto(orNormal, 2, 1, 2, 0180, 0620, 'BISCOITO RECH 335G');
     ImprimirTexto(orNormal, 2, 1, 1, 0140, 0620, 'CHOC BRANCO');
     ImprimirBarras(orNormal, 'F', '2', '2', 0020, 0620, '7896003701685', 070);

     Imprimir(StrToInt(eCopias.Text), StrToInt(eAvanco.Text));
     Desativar;
  end;
end;

procedure TFPrincipal.Button1Click(Sender: TObject);
begin
  with ACBrETQ do
  begin
     Modelo := TACBrETQModelo(cbModelo.ItemIndex);
     Porta := cbPorta.Text;
     Avanco := StrToInt(eAvanco.Text);
     LimparMemoria := ckMemoria.Checked ;
     Ativar;

     ImprimirImagem(1,10,10,Edit1.Text);

     Imprimir(StrToInt(eCopias.Text), StrToInt(eAvanco.Text));
     Desativar;
  end ;
end;

procedure TFPrincipal.Button2Click(Sender: TObject);
begin
  if OpenPictureDialog1.Execute then
  begin
     Image1.Picture.LoadFromFile(OpenPictureDialog1.FileName);
  end ;

  with ACBrETQ do
  begin
     Modelo := TACBrETQModelo(cbModelo.ItemIndex);
     Porta := cbPorta.Text;
     LimparMemoria := ckMemoria.Checked ;
     Ativar;

     CarregarImagem(Image1.Picture.Bitmap, Edit1.Text, True);

     Desativar;
  end ;
end;

end.
