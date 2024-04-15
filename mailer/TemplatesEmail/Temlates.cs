namespace CadastroClienteApi.mailer.Templates;

    public class EmailTemplates
    {
        public const string CadastroEmAnaliseHtml = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Cadastro em Análise</title>
  <style>
    /* Estilos opcionais para melhorar a aparência do email */
    body {
      font-family: Arial, sans-serif;
      margin: 0;
      padding: 0;
      background-color: #f4f4f4;
    }
    .container {
      max-width: 600px;
      margin: 20px auto;
      padding: 20px;
      background-color: #fff;
      border-radius: 5px;
      box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    }
    h1 {
      color: #333;
    }
    p {
      margin-bottom: 20px;
      color: #666;
    }
    .footer {
      margin-top: 20px;
      font-size: 12px;
      color: #888;
      text-align: center;
    }
  </style>
</head>
<body>
  <div class=""container"">
    <p>Olá {{nome}},</p>
    <p>O seu cadastro está em análise e em breve você receberá um e-mail com novas atualizações sobre seu cadastro.</p>
    <p>Atenciosamente,</p>
    <p>Equipe PATHBIT</p>
  </div>
  <div class=""footer"">
    <p>Este é um email automático. Por favor, não responda a este email.</p>
  </div>
</body>
</html>";
        public const string CadastroEmAnaliseTxt = @"
Olá {{nome}},

O seu cadastro está em análise e em breve você receberá um e-mail com novas atualizações sobre seu cadastro.

Atenciosamente,
Equipe PATHBIT
";
    }
