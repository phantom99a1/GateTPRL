-- Start of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_HNX_CONFIRM
-- Generated 22-May-2024 16:46:05 from HNX_GATE_TPRL@DB_36

CREATE OR REPLACE 
PACKAGE pkg_msg_tprl_hnx_confirm
  IS
    PROCEDURE proc_insert
    (
         p_id IN msg_tprl_hnx_confirm.id % TYPE,
         p_sor IN msg_tprl_hnx_confirm.sor % TYPE,
         p_msgtype IN msg_tprl_hnx_confirm.msgtype % TYPE,
         p_sendercompid IN msg_tprl_hnx_confirm.sendercompid % TYPE,
         p_targetcompid IN msg_tprl_hnx_confirm.targetcompid % TYPE,
         p_msgseqnum IN msg_tprl_hnx_confirm.msgseqnum % TYPE,
         p_possdupflag IN msg_tprl_hnx_confirm.possdupflag % TYPE,
         p_sendingtime IN msg_tprl_hnx_confirm.sendingtime % TYPE,
         p_text IN msg_tprl_hnx_confirm.text % TYPE,
         p_exectype IN msg_tprl_hnx_confirm.exectype % TYPE,
         p_lastmsgseqnumprocessed IN msg_tprl_hnx_confirm.lastmsgseqnumprocessed % TYPE,
         p_ordstatus IN msg_tprl_hnx_confirm.ordstatus % TYPE,
         p_orderid IN msg_tprl_hnx_confirm.orderid % TYPE,
         p_clordid IN msg_tprl_hnx_confirm.clordid % TYPE,
         p_symbol IN msg_tprl_hnx_confirm.symbol % TYPE,
         p_side IN msg_tprl_hnx_confirm.side % TYPE,
         p_orderqty IN msg_tprl_hnx_confirm.orderqty % TYPE,
         p_ordtype IN msg_tprl_hnx_confirm.ordtype % TYPE,
         p_price IN msg_tprl_hnx_confirm.price % TYPE,
         p_account IN msg_tprl_hnx_confirm.account % TYPE,
         p_settlvalue IN msg_tprl_hnx_confirm.settlvalue % TYPE,
         p_leavesqty IN msg_tprl_hnx_confirm.leavesqty % TYPE,
         p_origclordid IN msg_tprl_hnx_confirm.origclordid % TYPE,
         p_lastqty IN msg_tprl_hnx_confirm.lastqty % TYPE,
         p_lastpx IN msg_tprl_hnx_confirm.lastpx % TYPE,
         p_execid IN msg_tprl_hnx_confirm.execid % TYPE,
         p_reciprocalmember IN msg_tprl_hnx_confirm.reciprocalmember % TYPE,
         p_ordrejreason IN msg_tprl_hnx_confirm.ordrejreason % TYPE,
         p_underlyinglastqty IN msg_tprl_hnx_confirm.underlyinglastqty % TYPE,
         p_remark IN msg_tprl_hnx_confirm.remark % TYPE,
         p_lastchange IN msg_tprl_hnx_confirm.lastchange % TYPE,
         p_createtime IN msg_tprl_hnx_confirm.createtime % TYPE,
         p_orderno IN msg_tprl_hnx_confirm.orderno % TYPE,
         p_return OUT NUMBER
    );
END;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_msg_tprl_hnx_confirm
IS
    PROCEDURE proc_insert
    (
        p_id IN msg_tprl_hnx_confirm.id % TYPE,
        p_sor IN msg_tprl_hnx_confirm.sor % TYPE,
        p_msgtype IN msg_tprl_hnx_confirm.msgtype % TYPE,
        p_sendercompid IN msg_tprl_hnx_confirm.sendercompid % TYPE,
        p_targetcompid IN msg_tprl_hnx_confirm.targetcompid % TYPE,
        p_msgseqnum IN msg_tprl_hnx_confirm.msgseqnum % TYPE,
        p_possdupflag IN msg_tprl_hnx_confirm.possdupflag % TYPE,
        p_sendingtime IN msg_tprl_hnx_confirm.sendingtime % TYPE,
        p_text IN msg_tprl_hnx_confirm.text % TYPE,
        p_exectype IN msg_tprl_hnx_confirm.exectype % TYPE,
        p_lastmsgseqnumprocessed IN msg_tprl_hnx_confirm.lastmsgseqnumprocessed % TYPE,
        p_ordstatus IN msg_tprl_hnx_confirm.ordstatus % TYPE,
        p_orderid IN msg_tprl_hnx_confirm.orderid % TYPE,
        p_clordid IN msg_tprl_hnx_confirm.clordid % TYPE,
        p_symbol IN msg_tprl_hnx_confirm.symbol % TYPE,
        p_side IN msg_tprl_hnx_confirm.side % TYPE,
        p_orderqty IN msg_tprl_hnx_confirm.orderqty % TYPE,
        p_ordtype IN msg_tprl_hnx_confirm.ordtype % TYPE,
        p_price IN msg_tprl_hnx_confirm.price % TYPE,
        p_account IN msg_tprl_hnx_confirm.account % TYPE,
        p_settlvalue IN msg_tprl_hnx_confirm.settlvalue % TYPE,
        p_leavesqty IN msg_tprl_hnx_confirm.leavesqty % TYPE,
        p_origclordid IN msg_tprl_hnx_confirm.origclordid % TYPE,
        p_lastqty IN msg_tprl_hnx_confirm.lastqty % TYPE,
        p_lastpx IN msg_tprl_hnx_confirm.lastpx % TYPE,
        p_execid IN msg_tprl_hnx_confirm.execid % TYPE,
        p_reciprocalmember IN msg_tprl_hnx_confirm.reciprocalmember % TYPE,
        p_ordrejreason IN msg_tprl_hnx_confirm.ordrejreason % TYPE,
        p_underlyinglastqty IN msg_tprl_hnx_confirm.underlyinglastqty % TYPE,
        p_remark IN msg_tprl_hnx_confirm.remark % TYPE,
        p_lastchange IN msg_tprl_hnx_confirm.lastchange % TYPE,
        p_createtime IN msg_tprl_hnx_confirm.createtime % TYPE,
        p_orderno IN msg_tprl_hnx_confirm.orderno % TYPE,
        p_return OUT NUMBER
    )
    IS
        v_id NUMBER;
    BEGIN

        p_return := 0;
        SELECT SEQ_MSG_TPRL_HNX_CONFIRM.nextval INTO v_id FROM dual;
        INSERT INTO msg_tprl_hnx_confirm
        (
            id, sor, msgtype, sendercompid, targetcompid, msgseqnum, possdupflag, sendingtime, text, exectype, lastmsgseqnumprocessed, ordstatus, orderid, clordid, symbol, side, orderqty, ordtype, price, account, settlvalue, leavesqty, origclordid, lastqty, lastpx, execid, reciprocalmember, ordrejreason, underlyinglastqty, remark, lastchange, createtime,orderno

        )
        VALUES
        (
            v_id, p_sor, p_msgtype, p_sendercompid, p_targetcompid, p_msgseqnum, p_possdupflag, p_sendingtime, p_text, p_exectype, p_lastmsgseqnumprocessed, p_ordstatus, p_orderid, p_clordid, p_symbol, p_side, p_orderqty, p_ordtype, p_price, p_account, p_settlvalue, p_leavesqty, p_origclordid, p_lastqty, p_lastpx, p_execid, p_reciprocalmember, p_ordrejreason, p_underlyinglastqty, p_remark, p_lastchange, p_createtime,p_orderno

        );


        p_return := 1;
    COMMIT;
    EXCEPTION
    WHEN OTHERS THEN
        PKG_LOG.LOGMSG('TRACE:' ||  SUBSTR(DBMS_UTILITY.FORMAT_ERROR_BACKTRACE ,0,500) || ' CODE:'    ||SQLCODE  || ' SQLERRM:' ||SUBSTR(SQLERRM,1,500)) ;
        p_return := -1;
    END proc_insert;



   -- Enter further code below as specified in the Package spec.
END;
/


-- End of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_HNX_CONFIRM

