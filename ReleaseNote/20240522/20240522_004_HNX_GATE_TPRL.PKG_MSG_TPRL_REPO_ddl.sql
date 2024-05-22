-- Start of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_REPO
-- Generated 22-May-2024 16:44:30 from HNX_GATE_TPRL@DB_36

CREATE OR REPLACE 
PACKAGE pkg_msg_tprl_repo
  IS
    PROCEDURE proc_insert
    (
         p_id IN msg_tprl_repo.id % TYPE,
         p_sor IN msg_tprl_repo.sor % TYPE,
         p_msgtype IN msg_tprl_repo.msgtype % TYPE,
         p_msgseqnum IN msg_tprl_repo.msgseqnum % TYPE,
         p_sendercompid IN msg_tprl_repo.sendercompid % TYPE,
         p_sendingtime IN msg_tprl_repo.sendingtime % TYPE,
         p_targetcompid IN msg_tprl_repo.targetcompid % TYPE,
         p_possdupflag IN msg_tprl_repo.possdupflag % TYPE,
         p_text IN msg_tprl_repo.text % TYPE,
         p_partyid IN msg_tprl_repo.partyid % TYPE,
         p_copartyid IN msg_tprl_repo.copartyid % TYPE,
         p_matchreporttype IN msg_tprl_repo.matchreporttype % TYPE,
         p_orderid IN msg_tprl_repo.orderid % TYPE,
         p_buyorderid IN msg_tprl_repo.buyorderid % TYPE,
         p_sellorderid IN msg_tprl_repo.sellorderid % TYPE,
         p_repurchaserate IN msg_tprl_repo.repurchaserate % TYPE,
         p_repurchaseterm IN msg_tprl_repo.repurchaseterm % TYPE,
         p_noside IN msg_tprl_repo.noside % TYPE,
         p_lastmsgseqnumprocessed IN msg_tprl_repo.lastmsgseqnumprocessed % TYPE,
         p_quotetype IN msg_tprl_repo.quotetype % TYPE,
         p_multilegrpttypereq IN msg_tprl_repo.multilegrpttypereq % TYPE,
         p_ordtype IN msg_tprl_repo.ordtype % TYPE,
         p_rfqreqid IN msg_tprl_repo.rfqreqid % TYPE,
         p_orgorderid IN msg_tprl_repo.orgorderid % TYPE,
         p_quoteid IN msg_tprl_repo.quoteid % TYPE,
         p_side IN msg_tprl_repo.side % TYPE,
         p_orderqty IN msg_tprl_repo.orderqty % TYPE,
         p_effectivetime IN msg_tprl_repo.effectivetime % TYPE,
         p_coaccount IN msg_tprl_repo.coaccount % TYPE,
         p_settldate IN msg_tprl_repo.settldate % TYPE,
         p_registid IN msg_tprl_repo.registid % TYPE,
         p_clordid IN msg_tprl_repo.clordid % TYPE,
         p_settldate2 IN msg_tprl_repo.settldate2 % TYPE,
         p_enddate IN msg_tprl_repo.enddate % TYPE,
         p_settlmethod IN msg_tprl_repo.settlmethod % TYPE,
         p_orderpartyid IN msg_tprl_repo.orderpartyid % TYPE,
         p_inquirymember IN msg_tprl_repo.inquirymember % TYPE,
         p_remark IN msg_tprl_repo.remark % TYPE,
         p_lastchange IN msg_tprl_repo.lastchange % TYPE,
         p_createtime IN msg_tprl_repo.createtime % TYPE,
         p_account IN msg_tprl_repo.account % TYPE,
         p_orderno IN msg_tprl_repo.orderno % TYPE,
         p_return OUT NUMBER
    );

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_msg_tprl_repo
IS
    PROCEDURE proc_insert
    (
        p_id IN msg_tprl_repo.id % TYPE,
        p_sor IN msg_tprl_repo.sor % TYPE,
        p_msgtype IN msg_tprl_repo.msgtype % TYPE,
        p_msgseqnum IN msg_tprl_repo.msgseqnum % TYPE,
        p_sendercompid IN msg_tprl_repo.sendercompid % TYPE,
        p_sendingtime IN msg_tprl_repo.sendingtime % TYPE,
        p_targetcompid IN msg_tprl_repo.targetcompid % TYPE,
        p_possdupflag IN msg_tprl_repo.possdupflag % TYPE,
        p_text IN msg_tprl_repo.text % TYPE,
        p_partyid IN msg_tprl_repo.partyid % TYPE,
        p_copartyid IN msg_tprl_repo.copartyid % TYPE,
        p_matchreporttype IN msg_tprl_repo.matchreporttype % TYPE,
        p_orderid IN msg_tprl_repo.orderid % TYPE,
        p_buyorderid IN msg_tprl_repo.buyorderid % TYPE,
        p_sellorderid IN msg_tprl_repo.sellorderid % TYPE,
        p_repurchaserate IN msg_tprl_repo.repurchaserate % TYPE,
        p_repurchaseterm IN msg_tprl_repo.repurchaseterm % TYPE,
        p_noside IN msg_tprl_repo.noside % TYPE,
        p_lastmsgseqnumprocessed IN msg_tprl_repo.lastmsgseqnumprocessed % TYPE,
        p_quotetype IN msg_tprl_repo.quotetype % TYPE,
        p_multilegrpttypereq IN msg_tprl_repo.multilegrpttypereq % TYPE,
        p_ordtype IN msg_tprl_repo.ordtype % TYPE,
        p_rfqreqid IN msg_tprl_repo.rfqreqid % TYPE,
        p_orgorderid IN msg_tprl_repo.orgorderid % TYPE,
        p_quoteid IN msg_tprl_repo.quoteid % TYPE,
        p_side IN msg_tprl_repo.side % TYPE,
        p_orderqty IN msg_tprl_repo.orderqty % TYPE,
        p_effectivetime IN msg_tprl_repo.effectivetime % TYPE,
        p_coaccount IN msg_tprl_repo.coaccount % TYPE,
        p_settldate IN msg_tprl_repo.settldate % TYPE,
        p_registid IN msg_tprl_repo.registid % TYPE,
        p_clordid IN msg_tprl_repo.clordid % TYPE,
        p_settldate2 IN msg_tprl_repo.settldate2 % TYPE,
        p_enddate IN msg_tprl_repo.enddate % TYPE,
        p_settlmethod IN msg_tprl_repo.settlmethod % TYPE,
        p_orderpartyid IN msg_tprl_repo.orderpartyid % TYPE,
        p_inquirymember IN msg_tprl_repo.inquirymember % TYPE,
        p_remark IN msg_tprl_repo.remark % TYPE,
        p_lastchange IN msg_tprl_repo.lastchange % TYPE,
        p_createtime IN msg_tprl_repo.createtime % TYPE,
        p_account IN msg_tprl_repo.account % TYPE,
        p_orderno IN msg_tprl_repo.orderno % TYPE,
        p_return OUT NUMBER
    )
    IS
        v_id NUMBER;
    BEGIN

        p_return :=0;
        SELECT SEQ_MSG_TPRL_REPO.nextval INTO v_id FROM dual;
        INSERT INTO msg_tprl_repo
        (
            id, sor, msgtype, msgseqnum, sendercompid, sendingtime, targetcompid, possdupflag, text, partyid, copartyid, matchreporttype, orderid, buyorderid, sellorderid, repurchaserate, repurchaseterm, noside, lastmsgseqnumprocessed, quotetype, multilegrpttypereq, ordtype, rfqreqid, orgorderid, quoteid, side, orderqty, effectivetime, coaccount, settldate, registid, clordid, settldate2, enddate, settlmethod, orderpartyid, inquirymember, remark, lastchange, createtime,account,orderno

        )
        VALUES
        (
            v_id, p_sor, p_msgtype, p_msgseqnum, p_sendercompid, p_sendingtime, p_targetcompid, p_possdupflag, p_text, p_partyid, p_copartyid, p_matchreporttype, p_orderid, p_buyorderid, p_sellorderid, p_repurchaserate, p_repurchaseterm, p_noside, p_lastmsgseqnumprocessed, p_quotetype, p_multilegrpttypereq, p_ordtype, p_rfqreqid, p_orgorderid, p_quoteid, p_side, p_orderqty, p_effectivetime, p_coaccount, p_settldate, p_registid, p_clordid, p_settldate2, p_enddate, p_settlmethod, p_orderpartyid, p_inquirymember, p_remark, p_lastchange, p_createtime,p_account,p_orderno

        );

        p_return := v_id;
    COMMIT;
    EXCEPTION
    WHEN OTHERS THEN
        PKG_LOG.LOGMSG('TRACE:' ||  SUBSTR(DBMS_UTILITY.FORMAT_ERROR_BACKTRACE ,0,500) || ' CODE:'    ||SQLCODE  || ' SQLERRM:' ||SUBSTR(SQLERRM,1,500)) ;
        p_return := -1;
    END proc_insert;
END;
/


-- End of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_REPO

