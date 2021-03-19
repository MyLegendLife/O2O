IF object_id(N'[dbo].[FK_dbo.T_StockRuleProd_dbo.T_StockRule_StockRuleId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[T_StockRuleProd] DROP CONSTRAINT [FK_dbo.T_StockRuleProd_dbo.T_StockRule_StockRuleId]
IF object_id(N'[dbo].[FK_dbo.T_StockRuleShop_dbo.T_StockRule_StockRuleId]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[T_StockRuleShop] DROP CONSTRAINT [FK_dbo.T_StockRuleShop_dbo.T_StockRule_StockRuleId]
ALTER TABLE [dbo].[T_StockRule] DROP CONSTRAINT [PK_dbo.T_StockRule]
ALTER TABLE [dbo].[T_StockRuleProd] DROP CONSTRAINT [PK_dbo.T_StockRuleProd]
ALTER TABLE [dbo].[T_StockRuleShop] DROP CONSTRAINT [PK_dbo.T_StockRuleShop]
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.T_StockRule')
AND col_name(parent_object_id, parent_column_id) = 'Id';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[T_StockRule] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[T_StockRule] ALTER COLUMN [Id] [uniqueidentifier] NOT NULL
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.T_StockRuleProd')
AND col_name(parent_object_id, parent_column_id) = 'Id';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[T_StockRuleProd] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[T_StockRuleProd] ALTER COLUMN [Id] [uniqueidentifier] NOT NULL
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.T_StockRuleShop')
AND col_name(parent_object_id, parent_column_id) = 'Id';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[T_StockRuleShop] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[T_StockRuleShop] ALTER COLUMN [Id] [uniqueidentifier] NOT NULL
ALTER TABLE [dbo].[T_StockRule] ADD CONSTRAINT [PK_dbo.T_StockRule] PRIMARY KEY ([Id])
ALTER TABLE [dbo].[T_StockRuleProd] ADD CONSTRAINT [PK_dbo.T_StockRuleProd] PRIMARY KEY ([Id])
ALTER TABLE [dbo].[T_StockRuleShop] ADD CONSTRAINT [PK_dbo.T_StockRuleShop] PRIMARY KEY ([Id])
ALTER TABLE [dbo].[T_StockRuleProd] ADD CONSTRAINT [FK_dbo.T_StockRuleProd_dbo.T_StockRule_StockRuleId] FOREIGN KEY ([StockRuleId]) REFERENCES [dbo].[T_StockRule] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[T_StockRuleShop] ADD CONSTRAINT [FK_dbo.T_StockRuleShop_dbo.T_StockRule_StockRuleId] FOREIGN KEY ([StockRuleId]) REFERENCES [dbo].[T_StockRule] ([Id]) ON DELETE CASCADE
INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
VALUES (N'202011200921543_202011201718', N'O2O.Model.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB6E24B7117D0F907F68CC5312AC352BAD1D3882E44096761DC1D64AD8919DBC2DA86E6AD4705FC6DDEC8D84205F96877C527E21EC3BEFB7BECC8C24E84543B24F91D5C56215C9AEFADF7FFE7BF2D7C738F2BEC02C0FD3E4747178F076E1C1C44F8330599F2E0A74FFD5B78BBF7EF7FBDF9DBC0FE247EF97B6DDBBB21D7E32C94F170F086D8E97CBDC7F8031C80FE2D0CFD23CBD47077E1A2F41902E8FDEBEFDCBF2F0700931C4026379DEC9A72241610CAB1FF8E7799AF870830A105DA5018CF2A61CD7AC2A54EF238861BE013E3C5D5C1F5D1F54AD16DE591402DC81158CEE171E4892140184BB77FC730E57284B93F56A830B4074FBB481B8DD3D8872D874FBB86F6E3A82B747E50896FD832D945FE4288D2D010FDF352C59B28F3B3176D1B10C33ED3D662E7A2A475D31EE74F13E829FCF7C3FC55CAFEB161E4BF5F83CCACA2708061F546D43981FE0E76378C0A1BCF1BAB66F3AD9C02254FEBDF1CE8B0815193C4D608132805BDC147751E8FF089F6ED35F61729A145144761B771CD75105B8E8264B3730434F9FE07D3398CB60E12DE9E796EC83DD63C433F5E87E2842FCFF474C1BDC45B0130A82112B9466F00798C00C2018DC008460969410B061DD5249AAE1D0C7B4A58845114FA68577051E7F82C91A3D9C2E8EF0ECF9103EC2A02D687AF17312E2A9D7F78AEBA61969FC632BC4619E57AF5641FCEB698857644BA20AD28786A4D594B01C66307FD8D238577E3AC318DF3F6E423CC80B2CFF2DADF2FFDB30B6EF31D6C5593FFB445D3EFA56D3679415F674CF3388BBDC77DB76181FC197705D2904F6153CA49B7CE17D8251559B3F849B7A09AAF463595B2BC75655E2C16569FC298D1A3DCCB7F87C0BB235C40D6F5365B3555A64BE454F4BD64B3B4A29F2CF7553BAA37C8BAE07644705CDDAF1901D3D59F6AB9276ADEA07EFBC50F510AFAB94489160F6289728534D62ABC030DD7E989709FAF3D7AECB9C965B5B5210DDBC1F534588669E429338CDBC72EE3ACDBAFEC17D9C6BF3AD49259F34C6D937D34CBB9FD275986C87F40DC8F37FA659F037903F6C8FFA0A44687EEAD8AF4D5A6A0ADAD310FF11AA463C91177001733F0B37B51FAB585C46B1537B153ED84C5D41F47DF17403323041B7A54B05613FC94D4A274B4D64522A0C3AD30E5F21757FFB7A4977850DB8DE8A5B893A6BBCB8B190964BDC150C5101126E80FBB8E0BD6E814CA4EECD94DFD138CAEFEF208C4178B6D9282D1953361BD25A413F532E2BA69CD56CE7E0A9B64E0B4C104BF58CD4661ADEB3DB1391EE3438AE07ACB7A35E359CD683EB2C70F47688275FB5BF683A815F21B9158B9DFC7747E3EF1B4EB480683746F0BF4673534DA612A26D8CEF16453759E8F7B33FC542E9E4D9E4453C14C5C0211EC95528295DA577613403AD0B18855F60E6A663C558674190C13C1FC38B34A2F7010E7DB557304E7137A79F481B340AA36FC0D370A5B542842FEA0671019E56F0B76118E720F161748E7938B9295393FA04413E87CD8BD7C022096619584D6AAE817D5F3C8D203BD5A2325C8AEBA1DF800C9DC568A01AB808F30D40FE8356CF9B1E989A92D32AFB71084E665257EFF20245E27D96B6B63187AB9FA4592DAAE77659848D066DB2D0882E7675F7F0AB692D20857F069A5D9589B6D14BC2DB393EC0943184CA299FE8A8720C33F912C1F83C19AA444B9455110F44E9D5FAF01E31EECB6E1DC036DA702CB5C96E462875AB93DABCC96029E64E5A937AF655690A486D6B27C140594F73E838645E19CB6CB94F729E26F7E1DA496CD9C75F2577872457BB073611DD2B7456A0B4128B2C1EE6C3BC8FE06858334D2894FABF7E2A22E8369FE8A75FA793CB749AC8842D5FCB0B38909CEEC66D2BDBE58A2AF687A9268D61D69591169EB221E721AB5BDB5E9EE89E945F21A69A188D43D4503E0E61EB412EBF8045AEAAEBD58E1D664F4EE8816F477D5DA14A3606FAAAD8141803A613D35D757B093D319D86649D61337D3A4CB3387F0D204078D52C22D1D6D9FB13CDEFE732A5862DD6D229A55CDA4DA6D4599EA77E58755BF8714DF7FD01CD8EF749E0997D8C50CB0BFF6103161E3C93C20D9E3BF8D7E9E24F1CCBB5343A1386A6C1DCE1A4C91C7264F014845939094054DE58C0933A4C103F5FC3C40F372032EA11F3B4E17C2F5F524787ADB9801B989473D588ED261D203E91E1FBD1916334928E59274B42A0D47226BEF7251301CD25B05E02F84BBCE672A6B979DC13213F839944BE943D9941BC94EC36A1DF3AD25B112DC99578957A31132EAD7AD169B11D112F755766525F7B2C60C24322D97B579F18F56F9D3D1937972CF5513E434126582C1FAF930B184104BD33BF8E58710E721F04BC69842D8B600499540D62068954BD2513F2DD71E7560452EDBAC9E4C6D08FEB0548B893632EA7863B6B027ABB2AB446239A417A8DDEA4493F287F6BBBA22C7499B4A2A5F69F04A2E5E692186EAEEEA328AB4634A728ABDEE42E8A72DD57FC0CC24FC0AC5D178EAECB22F888043B53D8886936A7F2E62A332B6F25E40A22DEB05D78BD432FB74E39F9E5F14A364BC0C8A9A141AA3FDAE150484B5683D0BB1C021CDEABD3A035F7763820CA0432C1C026810C86B0D53448CD0D1C011073AF4783D35F891040F1D72D7468FD46140FC6AA2B532CC93885968329A64444852A9CC124E62A2FF6CC169747345607E3609589F17658374472EE71BAC978E78B81EB06C2AE1034170C3824F9288F6790C12E8EC53E0E311E521D2818A4DEB221F0EA010C668CEC6B7BB1E858B146B30D61F6AE6D761CC6E78EF8F224CF1BBDFF6CEE4113A3E815B682294A6799051B81279A53359E3916BE9C8337478C90D1D90A9E99396E22E83119283E43513050EF4138F810A2516AF5B899BB309081ED194F6782767527CB3A486A5370B29444533DB9029B4D98AC89E8AA4D89B7AA43AB9E7FB5B20F3E1AD7184B3F17C420ED7ADB51426906D690A92DBF270DE08730CBD10540E00E948774E741CC35230C6E8979D112E26D6AFE05B62647FB4CF97F67D8B787C99CDDCDBB27CDF31FF0C8E2D2D7A98E63C52A5D86E195A16E410432C129F0791A157122F7BFE44F1381384810A2D81EABBA092242AB2AACF0FA78A10C5E5F618E47840025D18862732C3AC8270947D7982336813B49A8A6C81C830ACB49225115E678ED2E3B0925DB7997A3B087EF241A5BC7A39E2C99C9C3B9F2DC34E53648E8996FAC172ABDEEAE1408BFC44D23A800A65107ED3D0F4A0A9B323B14B61F6D99B53A61811407D772ACBD144189C360227EC4C68BBDE8A91E9E46ECFA280FACA6B15D338830862414516C8E45C72524E1E81A7BC43AD6A008B1AEB1106C227E2025D444B9395A15109084F991BD48A47E9EBA4D4EE250152ED3563665ADD41211C18F524D44F9CE4C7E6203C44D05707BA6F68A400FF1F28CD2B1459C0AD246E251150E786DA43221665B69613CD301D728039AAE72C1147596AF7D355C0D14876C37CD446790C723F6EA42F9F4349AA28F2446494E573AB7C48C633F7797494818E90D130577BA485E1477BA522B33A30EE6C5D81875E1360C3D325C178BD596DB6875222017ADD5890A6BBC2E289700B2ABB346AD426F0910AB7273B42EFE1609D5155A486B1B5F8B92D6B6D0C6387EE2A7725768310711672F3645169C6E626C515C6ECA2C660D11658B9A3844B92D5A1B6F8AC76B6BAC36B1BA6059CC1656576E8B26EA1F5D638ED8C7BC22D1FA524B8DCA4B17516C3BCE2EF8153FD0AECA42DE98205894DC31752EA8224DC9D7BE04CBA83C1F1C601CF5973E1CED2305C0342652FB5128A5559B324B144E34FB523BA43A5C138B5497DA2071B68DB561D3855FA218DC16DAE1540198589CAAD045B970BD62AAE63727F772D2B777AFDCE63C7D3FCB7ECA6B9E9F6E3775B82B338EDED84B9921EED9B9890D7717CF5E72F410BB2C3CE3F8C14CB41FCA4FA1AB2C8E4899B03FD4292953F7DCC55C7129C648CA994BA20E42AE43D86519EF83F650AB7457BA0BC726FB288D03966BD135E3015239FFD2BD6BC67A175F85D6BD4DA195D61500F5A536BB3BC4671DF41E8FE27B0F39DE7ECF9501375444D7E707CC95BDBDABF272E489BB20CA36E9A83725DDEFEE8268733993BA355A71A2BC035A71206F2E8AB2B735EB260B0FB3EB4B18943735574F39F68F0FCA0607ABDFA2F32884A55FDB36B80249780F7354C7D5591CBD3D3C5A78675108F2FA9E6F730FF598FDAACAE862EAE1BBF2622A0CE225FBB8FDF5D61225CF036A5B499CB458975EEE3209E0E3E9E25FDEBFE70F215424E16F050CAB7840F76179F55D1B32C831F15A1746E80BC8FC07907171438727741B0F994D582F42FEDA1E99CB462FC23DA4704D529D8852CF8FD4632AAFFC38BD1524910FF0FF687812F9A4EBA07C8E95CF1D7B97FFF85C3FFAC6AB76C48EBDB778F67199D64609376538B8F1F2A03F6F7D42C724D38BA431683F88BB701D966BD2D0F4E33C330CE4B243A1457327455199187C9B629808E5C2614AB389C844B8DFD8CB1B97767B245C514EED91A1C984D92341F3D9B0C5C0F6C844AAEB918C0341E464B10EB25E17F9A4D5CECB229FB4DAA98F63E6507EDE8BD2968D5C87396728C747F6722CC83FACE78615309D8D57CF1013706166E1B1A1A7E8F84B3481D5397A9FB7AA61B3E8BA58AAB4CC8CA4AB68C3BC15C43FC4E0F18FB632CD64911AA9836C86DBFB2805F6BC6312DCBA819898950E26049FCC76246041E65A67E3449CB9D6C0E2B3E82891A6D6ED0531596A8749339D89D699714C265A97894FE5127501A0B3D0BA20F03968C7590745096747B268F8ECB2E37459944A76A42EB379635DDE149735D605449833D66D4E8A53C6EA373A8DF486243FEC38E8335A45AA24ABCFDB306A0FCC8DE791539293B1765398ECA523ED638E60DF309949DD4188C4A46E20C2BCA46E508C41E9B431DB60ECC1B6AC2E6DE8F3D60393383726CA6547A5C12021E7AB404C700CE5002ACC73E9627589B35C3A99EBF3C8A836C7E54B1751071B834D2FB9F33BC733CBDA0B5E23F5CB99A34D3B81BC31C9FEDC0C4036D79F1B8A202F99932949E0EC813969966FEF794F18FD72EF20D92F429C46C935C79C2ED71D73CDFD260BB43D286FDD381999E64F22F70CB3C691B79288376A935F4B1E6A7C60DA3927315186F51B5D3E0C2288F134F729F3DBA8F231503BEDA584B8A9AD3DCBDD26789B7365529385AD1F9438CE3D3DDB7C82A58DB5C153DCEB0C6C82D73A67C62865A8FFE129E08625A49A4FEACC3E1FE5C9EE7DD6B4DD133F8931EE94B66DEFC4CFD228DF8D4C677C7E06F625371F26CA6C35B39466F57784D819BD2B9DF0DA0955253F129125854748936C2023284AFA21CC8B26A544568AA8DC7E16E6E56169F0BE0747896F22A227CFB924CE8D262547D58A47264EAF2324439836624A4403053141622059A2362939A65E4CADCBF6A621C61FC771F4F82662926462381D5556AFF344D916129AD2143552924ADE0A5B69489BF159B466C8C9EBE63E415EA802B691724E38FF258965F81542BE0DC6002912EDED508E395EE7B2590D542C9069537140F4E16C9830A3DC30460C94AB1D4B1F2718822217A8D2F56621E47940F7215B9C60549AE49F7A7F518445D68FC7A6D972C28DC526897615D61BB3C922F31B1FC0033B02781287717D56839D923C5CF71027E52799D0A75C80AECD65729FB6EE08D3A3B609779B1E8100FB0767190AEF818F70751962A1FAE0F3171015A5DE89EF6070995C176853203C6418DF4514334A8F4645BF4A6F47F7F9E4BABAA5908F3104DCCDB03C09BB4EBE2FC228E8FAFD4170122681285DA5E648B27C97A83C9A5C3F75481FD3C410A8615FE7E1DDC2781361B0FC3A59812FD0A56F7875F809AE81FF74D3C4619183E85F04CDF6938B10AC3310E70D46FF3CFE896538881FBFFB3F46871F2E71C60000 , N'6.4.4')

