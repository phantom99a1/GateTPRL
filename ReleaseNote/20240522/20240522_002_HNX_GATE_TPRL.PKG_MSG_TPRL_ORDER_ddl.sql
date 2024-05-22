-- Start of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_ORDER
-- Generated 22-May-2024 16:41:08 from HNX_GATE_TPRL@DB_36

CREATE OR REPLACE 
PACKAGE pkg_msg_tprl_order
  IS
    PROCEDURE proc_insert
    (
         p_id IN msg_tprl_order.id % TYPE,
         p_sor IN msg_tprl_order.sor % TYPE,
         p_msgtype IN msg_tprl_order.msgtype % TYPE,
         p_sendercompid IN msg_tprl_order.sendercompid % TYPE,
         p_targetcompid IN msg_tprl_order.targetcompid % TYPE,
         p_msgseqnum IN msg_tprl_order.msgseqnum % TYPE,
         p_sendingtime IN msg_tprl_order.sendingtime % TYPE,
         p_lastmsgseqnumprocessed IN msg_tprl_order.lastmsgseqnumprocessed % TYPE,
         p_text IN msg_tprl_order.text % TYPE,
         p_clordid IN msg_tprl_order.clordid % TYPE,
         p_account IN msg_tprl_order.account % TYPE,
         p_symbol IN msg_tprl_order.symbol % TYPE,
         p_side IN msg_tprl_order.side % TYPE,
         p_orderqty IN msg_tprl_order.orderqty % TYPE,
         p_ordtype IN msg_tprl_order.ordtype % TYPE,
         p_price2 IN msg_tprl_order.price2 % TYPE,
         p_price IN msg_tprl_order.price % TYPE,
         p_orderqty2 IN msg_tprl_order.orderqty2 % TYPE,
         p_origclordid IN msg_tprl_order.origclordid % TYPE,
         p_orgorderqty IN msg_tprl_order.orgorderqty % TYPE,
         p_special_type IN msg_tprl_order.special_type % TYPE,
         p_remark IN msg_tprl_order.remark % TYPE,
         p_lastchange IN msg_tprl_order.lastchange % TYPE,
         p_createtime IN msg_tprl_order.createtime % TYPE,
         p_orderno IN msg_tprl_order.orderno % TYPE,
         p_return OUT NUMBER
    );

END;
/


CREATE OR REPLACE 
PACKAGE BODY pkg_msg_tprl_order
IS
    PROCEDURE proc_insert
    (
         p_id IN msg_tprl_order.id % TYPE,
         p_sor IN msg_tprl_order.sor % TYPE,
         p_msgtype IN msg_tprl_order.msgtype % TYPE,
         p_sendercompid IN msg_tprl_order.sendercompid % TYPE,
         p_targetcompid IN msg_tprl_order.targetcompid % TYPE,
         p_msgseqnum IN msg_tprl_order.msgseqnum % TYPE,
         p_sendingtime IN msg_tprl_order.sendingtime % TYPE,
         p_lastmsgseqnumprocessed IN msg_tprl_order.lastmsgseqnumprocessed % TYPE,
         p_text IN msg_tprl_order.text % TYPE,
         p_clordid IN msg_tprl_order.clordid % TYPE,
         p_account IN msg_tprl_order.account % TYPE,
         p_symbol IN msg_tprl_order.symbol % TYPE,
         p_side IN msg_tprl_order.side % TYPE,
         p_orderqty IN msg_tprl_order.orderqty % TYPE,
         p_ordtype IN msg_tprl_order.ordtype % TYPE,
         p_price2 IN msg_tprl_order.price2 % TYPE,
         p_price IN msg_tprl_order.price % TYPE,
         p_orderqty2 IN msg_tprl_order.orderqty2 % TYPE,
         p_origclordid IN msg_tprl_order.origclordid % TYPE,
         p_orgorderqty IN msg_tprl_order.orgorderqty % TYPE,
         p_special_type IN msg_tprl_order.special_type % TYPE,
         p_remark IN msg_tprl_order.remark % TYPE,
         p_lastchange IN msg_tprl_order.lastchange % TYPE,
         p_createtime IN msg_tprl_order.createtime % TYPE,
         p_orderno IN msg_tprl_order.orderno % TYPE,
         p_return OUT NUMBER
    )
    IS
         v_id NUMBER;
    BEGIN

        p_return := 0;
        SELECT SEQ_MSG_TPRL_ORDER.nextval INTO v_id FROM dual;
        INSERT INTO msg_tprl_order
        (
            id, sor, msgtype, sendercompid, targetcompid, msgseqnum, sendingtime, lastmsgseqnumprocessed, text, clordid, account, symbol, side, orderqty, ordtype, price2, price, orderqty2, origclordid, orgorderqty, special_type, remark, lastchange, createtime,orderno

        )
        VALUES
        (
            v_id, p_sor, p_msgtype, p_sendercompid, p_targetcompid, p_msgseqnum, p_sendingtime, p_lastmsgseqnumprocessed, p_text, p_clordid, p_account, p_symbol, p_side, p_orderqty, p_ordtype, p_price2, p_price, p_orderqty2, p_origclordid, p_orgorderqty, p_special_type, p_remark, p_lastchange, p_createtime,p_orderno

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


-- End of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_ORDER

