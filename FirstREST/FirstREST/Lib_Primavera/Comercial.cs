using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interop.ErpBS800;
using Interop.StdPlatBS800;
using Interop.StdBE800;
using Interop.StdPlatBE800;
using Interop.GcpBE800;
using Interop.IBasBS800;
using ADODB;
using Interop.IGcpBS800;
using FirstREST.Lib_Primavera.Model;
//using Interop.StdBESql800;
//using Interop.StdBSSql800;

namespace FirstREST.Lib_Primavera
{
    public class Comercial
    {

        

        public static bool start()
        {
            const string NomeEmpresa = "TECHBAGS";
            const string user = "TBAdmin";
            const string password = "techbags00";
            return PriEngine.InitializeCompany(NomeEmpresa, user, password);
        }

        # region Cliente

        public static Model.Cliente ValidaCliente(string useremail, string userpassword)
        {
             
            StdBELista objList;

            if (start() == true)
            {
                string password = PriEngine.Platform.Criptografia.Encripta(userpassword, 50);
                string select = "SELECT Cliente FROM CLIENTES where CDU_EMAIL LIKE '" + useremail +
                "' and CDU_PASSWORD LIKE '" + password + "'";
                objList = PriEngine.Engine.Consulta(select);

                if (objList.NoFim())
                    return null;
                string cod = objList.Valor("Cliente");
                return GetCliente(cod);

            }

                return null;
        }

        public static List<Model.Cliente> ListaClientes()
        {             
            StdBELista objList;

            Model.Cliente cli = new Model.Cliente();
            List<Model.Cliente> listClientes = new List<Model.Cliente>();


            if (start() == true)
            {
                objList = PriEngine.Engine.Consulta("SELECT Cliente, Nome, Fac_Mor, Fac_Local, Fac_Cp, Fac_Cploc, CDU_PASSWORD, CDU_EMAIL as Email, NumContrib as NumContribuinte, NomeFiscal FROM  CLIENTES");

                while (!objList.NoFim())
                {
                    cli = new Model.Cliente();
                    cli.id = objList.Valor("Cliente");
                    cli.name = objList.Valor("Nome");
                    cli.fiscal_name = objList.Valor("NomeFiscal");
                    cli.tax_id = objList.Valor("NumContribuinte");
                    cli.email = objList.Valor("Email");
                    cli.street = objList.Valor("Fac_Mor");
                    cli.city = objList.Valor("Fac_Local");
                    cli.zip_code1 = objList.Valor("Fac_Cp");
                    cli.zip_code2 = objList.Valor("Fac_Cploc");
                    //cli.password = PriEngine.Platform.Criptografia.Descripta(objList.Valor("CDU_PASSWORD"),50);

                    listClientes.Add(cli);
                    objList.Seguinte();

                }

                return listClientes;
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.Cliente GetCliente(string codCliente)
        {
            GcpBECliente objCli;


            Model.Cliente myCli = new Model.Cliente();

            if (start() == true)
            {

                if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == true)
                {
                    objCli = PriEngine.Engine.Comercial.Clientes.Consulta(codCliente);
                    myCli.id = objCli.get_Cliente();
                    myCli.name = objCli.get_Nome();
                    myCli.fiscal_name = objCli.get_NomeFiscal();
                    myCli.tax_id = objCli.get_NumContribuinte();
                    foreach (StdBECampo campo in objCli.get_CamposUtil())
                    {
                        if (campo.Nome.Equals("CDU_EMAIL"))
                        {
                            myCli.email = campo.Valor;
                            break;
                        }
                    }
                    myCli.street = objCli.get_Morada();
                    myCli.city = objCli.get_Localidade();
                    myCli.zip_code1 = objCli.get_CodigoPostal();
                    myCli.zip_code2 = objCli.get_LocalidadeCodigoPostal();

                    return myCli;
                }
                else
                {
                    return null;
                }
            }
            else
                return null;
        }

        public static Lib_Primavera.Model.RespostaErro UpdCliente(string id, Lib_Primavera.Model.Cliente cliente)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            GcpBECliente objCli = new GcpBECliente();

            try
            {
                if (start() == true)
                {

                    if (PriEngine.Engine.Comercial.Clientes.Existe(id) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {
                        StdBECampos cmps = new StdBECampos();
                        StdBECampo cmp = new StdBECampo();

                        objCli = PriEngine.Engine.Comercial.Clientes.Edita(id);
                        objCli.set_EmModoEdicao(true);

                        if (cliente.name != null)
                        {
                            objCli.set_Nome(cliente.name);
                            objCli.set_NomeFiscal(cliente.name);
                        }

                        if (cliente.tax_id != null)
                            objCli.set_NumContribuinte(cliente.tax_id);

                        if (cliente.email != null || cliente.password != null)
                        {
                            if (cliente.email != null)
                            {
                                if (existeEmail(id, cliente.email, false))
                                {
                                    erro.Erro = 1;
                                    erro.Descricao = "Email já registado";
                                    return erro;
                                }

                                cmp.Nome = "CDU_Email";
                                cmp.Valor = cliente.email;
                                cmps.Insere(cmp);
                            }

                            if (cliente.password != null)
                            {
                                cmp = new StdBECampo();
                                cmp.Nome = "CDU_Password";
                                cmp.Valor = PriEngine.Platform.Criptografia.Encripta(cliente.password, 50);
                                cmps.Insere(cmp);
                            }

                            objCli.set_CamposUtil(cmps);
                        }

                        if (cliente.street != null)
                            objCli.set_Morada(cliente.street);

                        if (cliente.city != null)
                            objCli.set_Localidade(cliente.city);

                        if (cliente.zip_code1 != null)
                            objCli.set_CodigoPostal(cliente.zip_code1);

                        if (cliente.zip_code2 != null)
                            objCli.set_LocalidadeCodigoPostal(cliente.zip_code2);

                        PriEngine.Engine.Comercial.Clientes.Actualiza(objCli);

                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;

                }

            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }


        public static Lib_Primavera.Model.RespostaErro DelCliente(string codCliente)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBECliente objCli = new GcpBECliente();


            try
            {

                if (start() == true)
                {
                    if (PriEngine.Engine.Comercial.Clientes.Existe(codCliente) == false)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "O cliente não existe";
                        return erro;
                    }
                    else
                    {

                        PriEngine.Engine.Comercial.Clientes.Remove(codCliente);
                        erro.Erro = 0;
                        erro.Descricao = "Sucesso";
                        return erro;
                    }
                }

                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir a empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }

        }

        public static bool existeEmail(string id, string email, bool insert)
        {
            StdBELista objList;
            objList = PriEngine.Engine.Consulta("SELECT Cliente FROM  CLIENTES WHERE CDU_EMAIL='" + email + "'");

            if (objList != null)
            {
                if (objList.Vazia())
                    return false;
                else if (!insert)
                {
                    while (!objList.NoFim())
                    {
                        if (objList.Valor("Cliente") == id)
                            return false;
                        objList.Seguinte();
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public static Lib_Primavera.Model.RespostaErro InsereClienteObj(Model.Cliente cli)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            GcpBECliente myCli = new GcpBECliente();
            StdBECampos cmps = new StdBECampos();
            StdBECampo cmp = new StdBECampo();

            try
            {
                if (start() == true)
                {
                    List<Model.Cliente> clientes = ListaClientes();
                    string new_id;

                    if (clientes.Count >= 2)
                    {

                        Model.Cliente last;

                        if (clientes[clientes.Count - 1].id == "VD")
                            last = clientes[clientes.Count - 2];
                        else
                            last = clientes[clientes.Count - 1];

                        int value = int.Parse(last.id.Substring(1, 3));
                        int new_value = value + 1;
                        new_id = "C" + new_value.ToString("D3");
                    }
                    else
                        new_id = "C001";

                    myCli.set_Cliente(new_id);

                    if (cli.name == null)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "É necessário nome";
                        return erro;
                    }
                    myCli.set_Nome(cli.name);
                    myCli.set_NomeFiscal(cli.name);

                    myCli.set_NumContribuinte(cli.tax_id);
                    myCli.set_Morada(cli.street);
                    myCli.set_Localidade(cli.city);
                    myCli.set_CodigoPostal(cli.zip_code1);
                    myCli.set_LocalidadeCodigoPostal(cli.zip_code2);
                    myCli.set_Moeda("EUR");

                    if (cli.email == null)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "É necessário email";
                        return erro;
                    }
                    else if (existeEmail("", cli.email, true))
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Email já registado";
                        return erro;
                    }
                    else
                    {
                        cmp.Nome = "CDU_Email";
                        cmp.Valor = cli.email;
                        cmps.Insere(cmp);
                    }


                    if (cli.password == null)
                    {
                        erro.Erro = 1;
                        erro.Descricao = "É necessário password";
                        return erro;
                    }
                    else
                    {
                        cmp = new StdBECampo();
                        cmp.Nome = "CDU_PASSWORD";
                        cmp.Valor = PriEngine.Platform.Criptografia.Encripta(cli.password, 50);
                        cmps.Insere(cmp);
                    }

                    myCli.set_CamposUtil(cmps);
                    PriEngine.Engine.Comercial.Clientes.Actualiza(myCli);

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;
                }
            }

            catch (Exception ex)
            {
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }


        }

        #endregion Cliente;   // -----------------------------  END   CLIENTE    -----------------------

        #region Artigo

        public static Lib_Primavera.Model.Artigo GetArtigo(string codArtigo)
        {

            GcpBEArtigo objArtigo = new GcpBEArtigo();
            Model.Artigo myArt = new Model.Artigo();

            if (start() == true)
            {

                if (PriEngine.Engine.Comercial.Artigos.Existe(codArtigo) == false)
                {
                    return null;
                }
                else
                {
                    objArtigo = PriEngine.Engine.Comercial.Artigos.Consulta(codArtigo);
                    string pai = objArtigo.get_ArtigoPai();
                    if (pai.Length>0)
                        return null;
                    myArt.id = objArtigo.get_Artigo();
                    myArt.name = objArtigo.get_Descricao();
                    myArt.category = objArtigo.get_SubFamilia();
                    myArt.brand = objArtigo.get_Marca();
                    myArt.price = PriEngine.Engine.Comercial.ArtigosPrecos.ListaArtigosMoedas(codArtigo)[1].get_PVP1();
                    foreach(StdBECampo campo in objArtigo.get_CamposUtil())
                    {
                        if (campo.Nome.Equals("CDU_MATERIAL"))
                            myArt.material = campo.Valor;
                        else myArt.description = campo.Valor;
                    }
                    List<Model.SubProduct> subproducts = new List<Model.SubProduct>();

                    int sublength = PriEngine.Engine.Comercial.Artigos.EditaDimensoes(codArtigo).NumItens;
                    for (int i = 1; i <= sublength; i++)
                    {
                        Model.SubProduct sub = new Model.SubProduct();
                        sub.color = PriEngine.Engine.Comercial.Artigos.EditaDimensoes(codArtigo)[i].get_RubricaDimensao1();
                        sub.size = PriEngine.Engine.Comercial.Artigos.EditaDimensoes(codArtigo)[i].get_RubricaDimensao2();
                        sub.id = PriEngine.Engine.Comercial.Artigos.EditaDimensoes(codArtigo)[i].get_Artigo();
                        sub.stock = (int)PriEngine.Engine.Comercial.ArtigosArmazens.DaStockDisponivelArtigoArmazem(sub.id, "ACENT", "");
                        subproducts.Add(sub);
                    }

                    myArt.subproducts = subproducts.ToArray();

                    StdBELista pics = PriEngine.Engine.Consulta("Select * from Anexos where Tabela=4 and Chave='" + codArtigo + "'");

                    List<string> images_path = new List<string>();

                    while (!pics.NoFim())
                    {
                        string tipo = pics.Valor("Tipo");
                        string path = pics.Valor("Id");
                        if (tipo.Equals("IPR"))
                            images_path.Insert(0, path);
                        else images_path.Add(path);
                        pics.Seguinte();
                    }

                    myArt.image_links = images_path.ToArray();
                    
                        return myArt;
                }
                
            }
            else
            {
                return null;
            }

        }

        public static List<Model.Artigo> ListaArtigos(string category = null)
        {           
            StdBELista objList;

            List<Model.Artigo> listArts = new List<Model.Artigo>();

            if (start() == true)
            {

                objList = PriEngine.Engine.Comercial.Artigos.LstArtigos();

                while (!objList.NoFim())
                {
                    string id = objList.Valor("artigo");
                    Model.Artigo art = GetArtigo(id);
                    if(art!=null && ( art.category.Equals(category) || category == null))
                        listArts.Add(art);
                    objList.Seguinte();
                }

                return listArts;

            }
            else
            {
                return null;

            }

        }
        #endregion


        //------------------------------------ ENCOMENDA ---------------------
        /*
        public static Model.RespostaErro TransformaDoc(Model.DocCompra dc)
        {

            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            GcpBEDocumentoCompra objEnc = new GcpBEDocumentoCompra();
            GcpBEDocumentoCompra objGR = new GcpBEDocumentoCompra();
            GcpBELinhasDocumentoCompra objLinEnc = new GcpBELinhasDocumentoCompra();
            PreencheRelacaoCompras rl = new PreencheRelacaoCompras();

            List<Model.LinhaDocCompra> lstlindc = new List<Model.LinhaDocCompra>();

            try
            {
                if (PriEngine.InitializeCompany("BELAFLOR", "sa", "123456") == true)
                {
                

                    objEnc = PriEngine.Engine.Comercial.Compras.Edita("000", "ECF", "2013", 3);

                    // --- Criar os cabeçalhos da GR
                    objGR.set_Entidade(objEnc.get_Entidade());
                    objEnc.set_Serie("2013");
                    objEnc.set_Tipodoc("ECF");
                    objEnc.set_TipoEntidade("F");

                    objGR = PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(objGR,rl);
 

                    // façam p.f. o ciclo para percorrer as linhas da encomenda que pretendem copiar
                     
                        double QdeaCopiar;
                        PriEngine.Engine.Comercial.Internos.CopiaLinha("C", objEnc, "C", objGR, lin.NumLinha, QdeaCopiar);
                       
                        // precisamos aqui de um metodo que permita actualizar a Qde Satisfeita da linha de encomenda.  Existe em VB mas ainda não sei qual é em c#
                       
                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Compras.Actualiza(objEnc, "");
                    PriEngine.Engine.Comercial.Compras.Actualiza(objGR, "");

                    PriEngine.Engine.TerminaTransaccao();

                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        
        
        }

        */




        // ------------------------ Documentos de Compra --------------------------//
/*
        public static List<Model.DocCompra> VGR_List()
        {
            ErpBS objMotor = new ErpBS();
            
            StdBELista objListCab;
            StdBELista objListLin;
            Model.DocCompra dc = new Model.DocCompra();
            List<Model.DocCompra> listdc = new List<Model.DocCompra>();
            Model.LinhaDocCompra lindc = new Model.LinhaDocCompra();
            List<Model.LinhaDocCompra> listlindc = new List<Model.LinhaDocCompra>(); 

            if (PriEngine.InitializeCompany("CENAS", "", "") == true)
            {
                objListCab = PriEngine.Engine.Consulta("SELECT id, NumDocExterno, Entidade, DataDoc, NumDoc, TotalMerc, Serie From CabecCompras where TipoDoc='VGR'");
                while (!objListCab.NoFim())
                {
                    dc = new Model.DocCompra();
                    dc.id = objListCab.Valor("id");
                    dc.NumDocExterno = objListCab.Valor("NumDocExterno");
                    dc.Entidade = objListCab.Valor("Entidade");
                    dc.NumDoc = objListCab.Valor("NumDoc");
                    dc.Data = objListCab.Valor("DataDoc");
                    dc.TotalMerc = objListCab.Valor("TotalMerc");
                    dc.Serie = objListCab.Valor("Serie");
                    objListLin = PriEngine.Engine.Consulta("SELECT idCabecCompras, Artigo, Descricao, Quantidade, Unidade, PrecUnit, Desconto1, TotalILiquido, PrecoLiquido, Armazem, Lote from LinhasCompras where IdCabecCompras='" + dc.id + "' order By NumLinha");
                    listlindc = new List<Model.LinhaDocCompra>();

                    while (!objListLin.NoFim())
                    {
                        lindc = new Model.LinhaDocCompra();
                        lindc.IdCabecDoc = objListLin.Valor("idCabecCompras");
                        lindc.CodArtigo = objListLin.Valor("Artigo");
                        lindc.DescArtigo = objListLin.Valor("Descricao");
                        lindc.Quantidade = objListLin.Valor("Quantidade");
                        lindc.Unidade = objListLin.Valor("Unidade");
                        lindc.Desconto = objListLin.Valor("Desconto1");
                        lindc.PrecoUnitario = objListLin.Valor("PrecUnit");
                        lindc.TotalILiquido = objListLin.Valor("TotalILiquido");
                        lindc.TotalLiquido = objListLin.Valor("PrecoLiquido");
                        lindc.Armazem = objListLin.Valor("Armazem");
                        lindc.Lote = objListLin.Valor("Lote");

                        listlindc.Add(lindc);
                        objListLin.Seguinte();
                    }

                    dc.LinhasDoc = listlindc;
                    
                    listdc.Add(dc);
                    objListCab.Seguinte();
                }
            }
            return listdc;
        }



        public static Model.RespostaErro VGR_New(Model.DocCompra dc)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();
            

            GcpBEDocumentoCompra myGR = new GcpBEDocumentoCompra();
            GcpBELinhaDocumentoCompra myLin = new GcpBELinhaDocumentoCompra();
            GcpBELinhasDocumentoCompra myLinhas = new GcpBELinhasDocumentoCompra();

            PreencheRelacaoCompras rl = new PreencheRelacaoCompras();
            List<Model.LinhaDocCompra> lstlindv = new List<Model.LinhaDocCompra>();

            try
            {
                if (PriEngine.InitializeCompany("CENAS", "", "") == true)
                {
                    // Atribui valores ao cabecalho do doc
                    //myEnc.set_DataDoc(dv.Data);
                    myGR.set_Entidade(dc.Entidade);
                    myGR.set_NumDocExterno(dc.NumDocExterno);
                    myGR.set_Serie(dc.Serie);
                    myGR.set_Tipodoc("VGR");
                    myGR.set_TipoEntidade("F");
                    // Linhas do documento para a lista de linhas
                    lstlindv = dc.LinhasDoc;
                    PriEngine.Engine.Comercial.Compras.PreencheDadosRelacionados(myGR, rl);
                    foreach (Model.LinhaDocCompra lin in lstlindv)
                    {
                        PriEngine.Engine.Comercial.Compras.AdicionaLinha(myGR, lin.CodArtigo, lin.Quantidade, lin.Armazem, "", lin.PrecoUnitario, lin.Desconto);
                    }


                    PriEngine.Engine.IniciaTransaccao();
                    PriEngine.Engine.Comercial.Compras.Actualiza(myGR, "Teste");
                    PriEngine.Engine.TerminaTransaccao();
                    erro.Erro = 0;
                    erro.Descricao = "Sucesso";
                    return erro;
                }
                else
                {
                    erro.Erro = 1;
                    erro.Descricao = "Erro ao abrir empresa";
                    return erro;

                }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }
        
        */

        // ------ Documentos de venda ----------------------

        #region venda
        
        public static Model.RespostaErro Encomendas_New(Model.DocVenda dv)
        {
            Lib_Primavera.Model.RespostaErro erro = new Model.RespostaErro();

            GcpBEDocumentoVenda myEnc = new GcpBEDocumentoVenda();
            
            GcpBELinhasDocumentoVenda myLinhas = new GcpBELinhasDocumentoVenda();
             
            DateTime time = DateTime.Now;

            try
            {

                if (dv.customer == null)
                {
                    erro.Erro = 1;
                    erro.Descricao = "No customer received";
                    return erro;
                }

                for (int i = 0; i < dv.lines.Count; i++)
                {
                    if(dv.lines[i].quantity <= 0)
                    {
                        erro.Erro = 1;
                        erro.Descricao = dv.lines[i].product_id + " - quantity error";
                        return erro;
                    }
                    for(int j = i+1; j < dv.lines.Count; j++)
                        if(dv.lines[i].product_id.Equals(dv.lines[j].product_id))
                        {
                            erro.Erro = 1;
                            erro.Descricao = "repeated product";
                            return erro;
                        }
                }

                    if (start() == true)
                    {
                        myEnc.set_Entidade(dv.customer);
                        myEnc.set_DataDoc(time);
                        myEnc.set_Serie("A");
                        myEnc.set_Tipodoc("ECL");
                        myEnc.set_TipoEntidade("C");
                        myEnc.set_Seccao("2");//vendas a retalho
                        myEnc.set_CondPag("1"); // pronto pagamento

                        myEnc = PriEngine.Engine.Comercial.Vendas.PreencheDadosRelacionados(myEnc, PreencheRelacaoVendas.vdDadosTodos);

                        foreach (Model.LinhaDocVenda lin in dv.lines)
                        {
                            double preco = PriEngine.Engine.Comercial.ArtigosPrecos.ListaArtigosMoedas(lin.product_id)[1].get_PVP1();
                           
                            if (PriEngine.Engine.Comercial.ArtigosArmazens.DaStockDisponivelArtigoArmazem(lin.product_id, "ACENT", "") < lin.quantity)
                            {
                                erro.Erro = 1;
                                erro.Descricao = lin.product_id + " - out of stock";
                            };

                            GcpBELinhasDocumentoVenda linhasArt = PriEngine.Engine.Comercial.Vendas.SugereArtigoLinhas(myEnc, lin.product_id, lin.quantity, "ACENT", "", preco);
                            for (int i = 1; i <= linhasArt.NumItens; i++)
                            {
                                GcpBELinhaDocumentoVenda l = linhasArt[i];
                                if (l.get_IdLinhaPai() != null)
                                {
                                    l.set_DataEntrega(time);
                                    l.set_QuantReservada(lin.quantity);
                                }
                                String outp = l.Conteudo;
                                myLinhas.Insere(l);
                            }
                        }

                        myEnc.set_Linhas(myLinhas);

                        if (dv.delivery_adress != null && dv.delivery_city != null && dv.delivery_zip1 != null && dv.delivery_zip2 != null)
                        {
                            myEnc.set_MoradaEntrega(dv.delivery_adress);
                            myEnc.set_CodPostalEntrega(dv.delivery_zip1);
                            myEnc.set_CodPostalLocalidadeEntrega(dv.delivery_zip2);
                            myEnc.set_LocalidadeEntrega(dv.delivery_city);
                        }

                        PriEngine.Engine.IniciaTransaccao();
                        PriEngine.Engine.Comercial.Vendas.Actualiza(myEnc);
                        PriEngine.Engine.TerminaTransaccao();
                        erro.Erro = 0;
                        erro.Descricao = myEnc.get_ID();
                        return erro;
                    }
                    else
                    {
                        erro.Erro = 1;
                        erro.Descricao = "Erro ao abrir empresa";
                        return erro;

                    }

            }
            catch (Exception ex)
            {
                PriEngine.Engine.DesfazTransaccao();
                erro.Erro = 1;
                erro.Descricao = ex.Message;
                return erro;
            }
        }
        

        public static List<Model.DocVendaForList> Encomendas_List(string customer)
        {            
            StdBELista objListCab;
            List<Model.DocVendaForList> listdv = new List<Model.DocVendaForList>();

            if (start() == true)
            {

                string query = "SELECT id From CabecDoc where TipoDoc='ECL' and Entidade = '" + customer + "'";
                objListCab = PriEngine.Engine.Consulta(query);
                while (!objListCab.NoFim())
                {
                    Model.DocVendaForList dv = new Model.DocVendaForList();
                    dv.id = objListCab.Valor("id");
                    GcpBEDocumentoVenda venda = PriEngine.Engine.Comercial.Vendas.EditaID(dv.id);
                    dv.date = venda.get_DataDoc();
                    dv.total = venda.get_TotalDocumento();
                    dv.state = venda.get_Estado();
                    listdv.Add(dv);
                    objListCab.Seguinte();
                }
            }
            return listdv;
        }

        
        public static Model.DocVenda Encomenda_Get(string id)
        {
            Model.DocVenda dv = new Model.DocVenda();
            Model.LinhaDocVenda lindv = new Model.LinhaDocVenda();
            List<Model.LinhaDocVenda> listlindv = new List<Model.LinhaDocVenda>();

            if (start() == true)
            {
                GcpBEDocumentoVenda venda = PriEngine.Engine.Comercial.Vendas.EditaID(id);

                if(venda==null)
                    return null;

                dv.id = id;
                dv.customer = venda.get_Entidade();
                dv.date = venda.get_DataDoc();
                dv.state = venda.get_Estado();
                dv.total = venda.get_TotalDocumento();
                GcpBECargaDescarga cd = venda.get_CargaDescarga();
                dv.delivery_adress = cd.MoradaEntrega;
                dv.delivery_city = cd.LocalidadeEntrega;
                dv.delivery_zip1 = cd.CodPostalEntrega;
                dv.delivery_zip2 = cd.CodPostalLocalidadeEntrega;

                GcpBELinhasDocumentoVenda linhas =  venda.get_Linhas();
                for(int i = 1; i<= linhas.NumItens; i++)
                {
                    GcpBELinhaDocumentoVenda linha = linhas[i];
                    string idlinhapai =  linha.get_IdLinhaPai();
                    string idlinha = linha.get_IdLinha();
                    if(!idlinhapai.Equals(""))
                    {
                        LinhaDocVenda l = new LinhaDocVenda();

                        string artigo = linha.get_Artigo();
                        bool existe = PriEngine.Engine.Comercial.Artigos.Existe(artigo);
                        GcpBEArtigo a = PriEngine.Engine.Comercial.Artigos.Consulta(artigo);
                        string pai = a.get_ArtigoPai();

                        l.product_id = pai;
                        l.color = PriEngine.Engine.Comercial.Artigos.EditaDimensao(artigo).get_RubricaDimensao1();
                        l.size = PriEngine.Engine.Comercial.Artigos.EditaDimensao(artigo).get_RubricaDimensao2();
                        l.unit_price = linha.get_PrecUnit();
                        l.quantity = linha.get_Quantidade();
                        l.total = linha.get_TotalIliquido() + linha.get_TotalIva();
                        listlindv.Add(l);
                    }
                }


                dv.lines = listlindv;
                return dv;
            }
            return null;
        }
        
    }
        #endregion
}