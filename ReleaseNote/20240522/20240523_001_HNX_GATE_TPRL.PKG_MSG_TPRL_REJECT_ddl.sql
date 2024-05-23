-- Start of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_REJECT
-- Generated 23-May-2024 10:51:27 from HNX_GATE_TPRL@DB_36

CREATE OR REPLACE 
PACKAGE pkg_msg_tprl_reject
  IS
    PROCEDURE proc_insert
    (
         p_id IN msg_tprl_reject.id % TYPE,
         p_sor IN msg_tprl_reject.sor % TYPE,
         p_msgtype IN msg_tprl_reject.msgtype % TYPE,
         p_sendercompid IN msg_tprl_reject.sendercompid % TYPE,
         p_targetcompid IN msg_tprl_reject.targetcompid % TYPE,
         p_msgseqnum IN msg_tprl_reject.msgseqnum % TYPE,
         p_possdupflag IN msg_tprl_reject.possdupflag % TYPE,
         p_sendingtime IN msg_tprl_reject.sendingtime % TYPE,
         p_text IN msg_tprl_reject.text % TYPE,
         p_refseqnum IN msg_tprl_reject.refseqnum % TYPE,
         p_lastmsgseqnumprocessed IN msg_tprl_reject.lastmsgseqnumprocessed % TYPE,
         p_sessionrejectreason IN msg_tprl_reject.sessionrejectreason % TYPE,
         p_refmsgtype IN msg_tprl_reject.refmsgtype % TYPE,
         p_remark IN msg_tprl_reject.remark % TYPE,
         p_lastchange IN msg_tprl_reject.lastchange % TYPE,
         p_createtime IN msg_tprl_reject.createtime % TYPE,
         p_orderno IN msg_tprl_order.orderno % TYPE,
         p_return OUT NUMBER
    );

END; -- Package spec
/


CREATE OR REPLACE 
PACKAGE BODY pkg_msg_tprl_reject
IS
     PROCEDURE proc_insert
    (
        p_id IN msg_tprl_reject.id % TYPE,
        p_sor IN msg_tprl_reject.sor % TYPE,
        p_msgtype IN msg_tprl_reject.msgtype % TYPE,
        p_sendercompid IN msg_tprl_reject.sendercompid % TYPE,
        p_targetcompid IN msg_tprl_reject.targetcompid % TYPE,
        p_msgseqnum IN msg_tprl_reject.msgseqnum % TYPE,
        p_possdupflag IN msg_tprl_reject.possdupflag % TYPE,
        p_sendingtime IN msg_tprl_reject.sendingtime % TYPE,
        p_text IN msg_tprl_reject.text % TYPE,
        p_refseqnum IN msg_tprl_reject.refseqnum % TYPE,
        p_lastmsgseqnumprocessed IN msg_tprl_reject.lastmsgseqnumprocessed % TYPE,
        p_sessionrejectreason IN msg_tprl_reject.sessionrejectreason % TYPE,
        p_refmsgtype IN msg_tprl_reject.refmsgtype % TYPE,
        p_remark IN msg_tprl_reject.remark % TYPE,
        p_lastchange IN msg_tprl_reject.lastchange % TYPE,
        p_createtime IN msg_tprl_reject.createtime % TYPE,
        p_orderno IN msg_tprl_order.orderno % TYPE,
        p_return OUT NUMBER
    )
    IS
        v_id NUMBER;
    BEGIN
        p_return := 0;
        SELECT SEQ_MSG_TPRL_REJECT.nextval INTO v_id FROM dual;
        INSERT INTO msg_tprl_reject
        (
            id, sor, msgtype, sendercompid, targetcompid, msgseqnum, possdupflag, sendingtime, text, refseqnum, lastmsgseqnumprocessed, sessionrejectreason, refmsgtype, remark, lastchange, createtime,orderno

        )
        VALUES
        (
            v_id, p_sor, p_msgtype, p_sendercompid, p_targetcompid, p_msgseqnum, p_possdupflag, p_sendingtime, p_text, p_refseqnum, p_lastmsgseqnumprocessed, p_sessionrejectreason, p_refmsgtype, p_remark, p_lastchange, p_createtime,p_orderno

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


-- End of DDL Script for Package HNX_GATE_TPRL.PKG_MSG_TPRL_REJECT

