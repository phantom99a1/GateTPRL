-- Start of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_OUTRIGHT
-- Generated 22-May-2024 16:42:56 from HNX_GATE_TPRL@DB_36

CREATE OR REPLACE 
PACKAGE pkg_msg_tprl_outright
  IS
    PROCEDURE proc_insert
    (
         p_id IN msg_tprl_outright.id % TYPE,
         p_sor IN msg_tprl_outright.sor % TYPE,
         p_msgtype IN msg_tprl_outright.msgtype % TYPE,
         p_sendercompid IN msg_tprl_outright.sendercompid % TYPE,
         p_targetcompid IN msg_tprl_outright.targetcompid % TYPE,
         p_msgseqnum IN msg_tprl_outright.msgseqnum % TYPE,
         p_sendingtime IN msg_tprl_outright.sendingtime % TYPE,
         p_text IN msg_tprl_outright.text % TYPE,
         p_ordtype IN msg_tprl_outright.ordtype % TYPE,
         p_crosstype IN msg_tprl_outright.crosstype % TYPE,
         p_clordid IN msg_tprl_outright.clordid % TYPE,
         p_crossid IN msg_tprl_outright.crossid % TYPE,
         p_account IN msg_tprl_outright.account % TYPE,
         p_coaccount IN msg_tprl_outright.coaccount % TYPE,
         p_partyid IN msg_tprl_outright.partyid % TYPE,
         p_copartyid IN msg_tprl_outright.copartyid % TYPE,
         p_orderqty IN msg_tprl_outright.orderqty % TYPE,
         p_effectivetime IN msg_tprl_outright.effectivetime % TYPE,
         p_side IN msg_tprl_outright.side % TYPE,
         p_symbol IN msg_tprl_outright.symbol % TYPE,
         p_price2 IN msg_tprl_outright.price2 % TYPE,
         p_settlvalue IN msg_tprl_outright.settlvalue % TYPE,
         p_settldate IN msg_tprl_outright.settldate % TYPE,
         p_settlmethod IN msg_tprl_outright.settlmethod % TYPE,
         p_lastmsgseqnumprocessed IN msg_tprl_outright.lastmsgseqnumprocessed % TYPE,
         p_orderid IN msg_tprl_outright.orderid % TYPE,
         p_origcrossid IN msg_tprl_outright.origcrossid % TYPE,
         p_registid IN msg_tprl_outright.registid % TYPE,
         p_rfqreqid IN msg_tprl_outright.rfqreqid % TYPE,
         p_quoterespid IN msg_tprl_outright.quoterespid % TYPE,
         p_quoteresptype IN msg_tprl_outright.quoteresptype % TYPE,
         p_quoteid IN msg_tprl_outright.quoteid % TYPE,
         p_quotecanceltype IN msg_tprl_outright.quotecanceltype % TYPE,
         p_orderpartyid IN msg_tprl_outright.orderpartyid % TYPE,
         p_quotereqid IN msg_tprl_outright.quotereqid % TYPE,
         p_quotetype IN msg_tprl_outright.quotetype % TYPE,
         p_remark IN msg_tprl_outright.remark % TYPE,
         p_lastchange IN msg_tprl_outright.lastchange % TYPE,
         p_createtime IN msg_tprl_outright.createtime % TYPE,
         p_orderno IN msg_tprl_order.orderno % TYPE,
         p_return OUT NUMBER
    );

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_msg_tprl_outright
IS
    PROCEDURE proc_insert
    (
        p_id IN msg_tprl_outright.id % TYPE,
        p_sor IN msg_tprl_outright.sor % TYPE,
        p_msgtype IN msg_tprl_outright.msgtype % TYPE,
        p_sendercompid IN msg_tprl_outright.sendercompid % TYPE,
        p_targetcompid IN msg_tprl_outright.targetcompid % TYPE,
        p_msgseqnum IN msg_tprl_outright.msgseqnum % TYPE,
        p_sendingtime IN msg_tprl_outright.sendingtime % TYPE,
        p_text IN msg_tprl_outright.text % TYPE,
        p_ordtype IN msg_tprl_outright.ordtype % TYPE,
        p_crosstype IN msg_tprl_outright.crosstype % TYPE,
        p_clordid IN msg_tprl_outright.clordid % TYPE,
        p_crossid IN msg_tprl_outright.crossid % TYPE,
        p_account IN msg_tprl_outright.account % TYPE,
        p_coaccount IN msg_tprl_outright.coaccount % TYPE,
        p_partyid IN msg_tprl_outright.partyid % TYPE,
        p_copartyid IN msg_tprl_outright.copartyid % TYPE,
        p_orderqty IN msg_tprl_outright.orderqty % TYPE,
        p_effectivetime IN msg_tprl_outright.effectivetime % TYPE,
        p_side IN msg_tprl_outright.side % TYPE,
        p_symbol IN msg_tprl_outright.symbol % TYPE,
        p_price2 IN msg_tprl_outright.price2 % TYPE,
        p_settlvalue IN msg_tprl_outright.settlvalue % TYPE,
        p_settldate IN msg_tprl_outright.settldate % TYPE,
        p_settlmethod IN msg_tprl_outright.settlmethod % TYPE,
        p_lastmsgseqnumprocessed IN msg_tprl_outright.lastmsgseqnumprocessed % TYPE,
        p_orderid IN msg_tprl_outright.orderid % TYPE,
        p_origcrossid IN msg_tprl_outright.origcrossid % TYPE,
        p_registid IN msg_tprl_outright.registid % TYPE,
        p_rfqreqid IN msg_tprl_outright.rfqreqid % TYPE,
        p_quoterespid IN msg_tprl_outright.quoterespid % TYPE,
        p_quoteresptype IN msg_tprl_outright.quoteresptype % TYPE,
        p_quoteid IN msg_tprl_outright.quoteid % TYPE,
        p_quotecanceltype IN msg_tprl_outright.quotecanceltype % TYPE,
        p_orderpartyid IN msg_tprl_outright.orderpartyid % TYPE,
        p_quotereqid IN msg_tprl_outright.quotereqid % TYPE,
        p_quotetype IN msg_tprl_outright.quotetype % TYPE,
        p_remark IN msg_tprl_outright.remark % TYPE,
        p_lastchange IN msg_tprl_outright.lastchange % TYPE,
        p_createtime IN msg_tprl_outright.createtime % TYPE,
        p_orderno IN msg_tprl_order.orderno % TYPE,
        p_return OUT NUMBER
    )
    IS
        v_id NUMBER;
    BEGIN

        p_return := 0;
        SELECT SEQ_MSG_TPRL_OUTRIGHT.nextval INTO v_id FROM dual;
        INSERT INTO msg_tprl_outright
        (
            id, sor, msgtype, sendercompid, targetcompid, msgseqnum, sendingtime, text, ordtype, crosstype, clordid, crossid, account, coaccount, partyid, copartyid, orderqty, effectivetime, side, symbol, price2, settlvalue, settldate, settlmethod, lastmsgseqnumprocessed, orderid, origcrossid, registid, rfqreqid, quoterespid, quoteresptype, quoteid, quotecanceltype, orderpartyid, quotereqid, quotetype, remark, lastchange, createtime,orderno

        )
        VALUES
        (
            v_id, p_sor, p_msgtype, p_sendercompid, p_targetcompid, p_msgseqnum, p_sendingtime, p_text, p_ordtype, p_crosstype, p_clordid, p_crossid, p_account, p_coaccount, p_partyid, p_copartyid, p_orderqty, p_effectivetime, p_side, p_symbol, p_price2, p_settlvalue, p_settldate, p_settlmethod, p_lastmsgseqnumprocessed, p_orderid, p_origcrossid, p_registid, p_rfqreqid, p_quoterespid, p_quoteresptype, p_quoteid, p_quotecanceltype, p_orderpartyid, p_quotereqid, p_quotetype, p_remark, p_lastchange, p_createtime,p_orderno

        );

        p_return := 1;
    COMMIT;
    EXCEPTION
    WHEN OTHERS THEN
        PKG_LOG.LOGMSG('TRACE:' ||  SUBSTR(DBMS_UTILITY.FORMAT_ERROR_BACKTRACE ,0,500) || ' CODE:'    ||SQLCODE  || ' SQLERRM:' ||SUBSTR(SQLERRM,1,500)) ;
        p_return := -1;
    END proc_insert;
END;
/


-- End of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_OUTRIGHT

