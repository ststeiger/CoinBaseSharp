

-- DELETE FROM t_map_currency_rate
SELECT 
	 cur_time 
	,MAX(AED) AS AED
	,MAX(AFN) AS AFN
	,MAX([ALL]) AS [ALL]
	,MAX(AMD) AS AMD
	,MAX(ANG) AS ANG
	,MAX(AOA) AS AOA
	,MAX(ARS) AS ARS
	,MAX(AUD) AS AUD
	,MAX(AWG) AS AWG
	,MAX(AZN) AS AZN
	,MAX(BAM) AS BAM
	,MAX(BBD) AS BBD
	,MAX(BDT) AS BDT
	,MAX(BGN) AS BGN
	,MAX(BHD) AS BHD
	,MAX(BIF) AS BIF
	,MAX(BMD) AS BMD
	,MAX(BND) AS BND
	,MAX(BOB) AS BOB
	,MAX(BRL) AS BRL
	,MAX(BSD) AS BSD
	,MAX(BTC) AS BTC
	,MAX(BTN) AS BTN
	,MAX(BWP) AS BWP
	,MAX(BYR) AS BYR
	,MAX(BZD) AS BZD
	,MAX(CAD) AS CAD
	,MAX(CDF) AS CDF
	,MAX(CHF) AS CHF
	,MAX(CLF) AS CLF
	,MAX(CLP) AS CLP
	,MAX(CNY) AS CNY
	,MAX(COP) AS COP
	,MAX(CRC) AS CRC
	,MAX(CUC) AS CUC
	,MAX(CUP) AS CUP
	,MAX(CVE) AS CVE
	,MAX(CZK) AS CZK
	,MAX(DJF) AS DJF
	,MAX(DKK) AS DKK
	,MAX(DOP) AS DOP
	,MAX(DZD) AS DZD
	,MAX(EEK) AS EEK
	,MAX(EGP) AS EGP
	,MAX(ERN) AS ERN
	,MAX(ETB) AS ETB
	,MAX(EUR) AS EUR
	,MAX(FJD) AS FJD
	,MAX(FKP) AS FKP
	,MAX(GBP) AS GBP
	,MAX(GEL) AS GEL
	,MAX(GGP) AS GGP
	,MAX(GHS) AS GHS
	,MAX(GIP) AS GIP
	,MAX(GMD) AS GMD
	,MAX(GNF) AS GNF
	,MAX(GTQ) AS GTQ
	,MAX(GYD) AS GYD
	,MAX(HKD) AS HKD
	,MAX(HNL) AS HNL
	,MAX(HRK) AS HRK
	,MAX(HTG) AS HTG
	,MAX(HUF) AS HUF
	,MAX(IDR) AS IDR
	,MAX(ILS) AS ILS
	,MAX(IMP) AS IMP
	,MAX(INR) AS INR
	,MAX(IQD) AS IQD
	,MAX(IRR) AS IRR
	,MAX(ISK) AS ISK
	,MAX(JEP) AS JEP
	,MAX(JMD) AS JMD
	,MAX(JOD) AS JOD
	,MAX(JPY) AS JPY
	,MAX(KES) AS KES
	,MAX(KGS) AS KGS
	,MAX(KHR) AS KHR
	,MAX(KMF) AS KMF
	,MAX(KPW) AS KPW
	,MAX(KRW) AS KRW
	,MAX(KWD) AS KWD
	,MAX(KYD) AS KYD
	,MAX(KZT) AS KZT
	,MAX(LAK) AS LAK
	,MAX(LBP) AS LBP
	,MAX(LKR) AS LKR
	,MAX(LRD) AS LRD
	,MAX(LSL) AS LSL
	,MAX(LTL) AS LTL
	,MAX(LVL) AS LVL
	,MAX(LYD) AS LYD
	,MAX(MAD) AS MAD
	,MAX(MDL) AS MDL
	,MAX(MGA) AS MGA
	,MAX(MKD) AS MKD
	,MAX(MMK) AS MMK
	,MAX(MNT) AS MNT
	,MAX(MOP) AS MOP
	,MAX(MRO) AS MRO
	,MAX(MTL) AS MTL
	,MAX(MUR) AS MUR
	,MAX(MVR) AS MVR
	,MAX(MWK) AS MWK
	,MAX(MXN) AS MXN
	,MAX(MYR) AS MYR
	,MAX(MZN) AS MZN
	,MAX(NAD) AS NAD
	,MAX(NGN) AS NGN
	,MAX(NIO) AS NIO
	,MAX(NOK) AS NOK
	,MAX(NPR) AS NPR
	,MAX(NZD) AS NZD
	,MAX(OMR) AS OMR
	,MAX(PAB) AS PAB
	,MAX(PEN) AS PEN
	,MAX(PGK) AS PGK
	,MAX(PHP) AS PHP
	,MAX(PKR) AS PKR
	,MAX(PLN) AS PLN
	,MAX(PYG) AS PYG
	,MAX(QAR) AS QAR
	,MAX(RON) AS RON
	,MAX(RSD) AS RSD
	,MAX(RUB) AS RUB
	,MAX(RWF) AS RWF
	,MAX(SAR) AS SAR
	,MAX(SBD) AS SBD
	,MAX(SCR) AS SCR
	,MAX(SDG) AS SDG
	,MAX(SEK) AS SEK
	,MAX(SGD) AS SGD
	,MAX(SHP) AS SHP
	,MAX(SLL) AS SLL
	,MAX(SOS) AS SOS
	,MAX(SRD) AS SRD
	,MAX(STD) AS STD
	,MAX(SVC) AS SVC
	,MAX(SYP) AS SYP
	,MAX(SZL) AS SZL
	,MAX(THB) AS THB
	,MAX(TJS) AS TJS
	,MAX(TMT) AS TMT
	,MAX(TND) AS TND
	,MAX([TOP]) AS [TOP]
	,MAX([TRY]) AS [TRY]
	,MAX(TTD) AS TTD
	,MAX(TWD) AS TWD
	,MAX(TZS) AS TZS
	,MAX(UAH) AS UAH
	,MAX(UGX) AS UGX
	,MAX(USD) AS USD
	,MAX(UYU) AS UYU
	,MAX(UZS) AS UZS
	,MAX(VEF) AS VEF
	,MAX(VND) AS VND
	,MAX(VUV) AS VUV
	,MAX(WST) AS WST
	,MAX(XAF) AS XAF
	,MAX(XAG) AS XAG
	,MAX(XAU) AS XAU
	,MAX(XCD) AS XCD
	,MAX(XDR) AS XDR
	,MAX(XOF) AS XOF
	,MAX(XPD) AS XPD
	,MAX(XPF) AS XPF
	,MAX(XPT) AS XPT
	,MAX(YER) AS YER
	,MAX(ZAR) AS ZAR
	,MAX(ZMK) AS ZMK
	,MAX(ZMW) AS ZMW
	,MAX(ZWL) AS ZWL
FROM t_map_currency_rate 
PIVOT
(
	MAX(cur_rate)
	FOR cur_name IN
	( 
		 AED
		,AFN
		,[ALL]
		,AMD
		,ANG
		,AOA
		,ARS
		,AUD
		,AWG
		,AZN
		,BAM
		,BBD
		,BDT
		,BGN
		,BHD
		,BIF
		,BMD
		,BND
		,BOB
		,BRL
		,BSD
		,BTC
		,BTN
		,BWP
		,BYR
		,BZD
		,CAD
		,CDF
		,CHF
		,CLF
		,CLP
		,CNY
		,COP
		,CRC
		,CUC
		,CUP
		,CVE
		,CZK
		,DJF
		,DKK
		,DOP
		,DZD
		,EEK
		,EGP
		,ERN
		,ETB
		,EUR
		,FJD
		,FKP
		,GBP
		,GEL
		,GGP
		,GHS
		,GIP
		,GMD
		,GNF
		,GTQ
		,GYD
		,HKD
		,HNL
		,HRK
		,HTG
		,HUF
		,IDR
		,ILS
		,IMP
		,INR
		,IQD
		,IRR
		,ISK
		,JEP
		,JMD
		,JOD
		,JPY
		,KES
		,KGS
		,KHR
		,KMF
		,KPW
		,KRW
		,KWD
		,KYD
		,KZT
		,LAK
		,LBP
		,LKR
		,LRD
		,LSL
		,LTL
		,LVL
		,LYD
		,MAD
		,MDL
		,MGA
		,MKD
		,MMK
		,MNT
		,MOP
		,MRO
		,MTL
		,MUR
		,MVR
		,MWK
		,MXN
		,MYR
		,MZN
		,NAD
		,NGN
		,NIO
		,NOK
		,NPR
		,NZD
		,OMR
		,PAB
		,PEN
		,PGK
		,PHP
		,PKR
		,PLN
		,PYG
		,QAR
		,RON
		,RSD
		,RUB
		,RWF
		,SAR
		,SBD
		,SCR
		,SDG
		,SEK
		,SGD
		,SHP
		,SLL
		,SOS
		,SRD
		,STD
		,SVC
		,SYP
		,SZL
		,THB
		,TJS
		,TMT
		,TND
		,[TOP]
		,[TRY]
		,TTD
		,TWD
		,TZS
		,UAH
		,UGX
		,USD
		,UYU
		,UZS
		,VEF
		,VND
		,VUV
		,WST
		,XAF
		,XAG
		,XAU
		,XCD
		,XDR
		,XOF
		,XPD
		,XPF
		,XPT
		,YER
		,ZAR
		,ZMK
		,ZMW
		,ZWL
	)
) AS pvt

GROUP BY cur_time 
	
-- ORDER BY cur_name 

