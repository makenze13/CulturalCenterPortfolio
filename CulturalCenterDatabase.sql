--
-- PostgreSQL database dump
--

\restrict khruYodPIZqHuj4sauabg09MaLVAv8R0Xr9q9noD627YNluscEflbNdDMVheK95

-- Dumped from database version 18.6
-- Dumped by pg_dump version 18.6


SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 24603)
-- Name: pgcrypto; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA public;


--
-- TOC entry 5121 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION pgcrypto; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pgcrypto IS 'cryptographic functions';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 221 (class 1259 OID 24578)
-- Name: Employees; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Employees" (
    "IdEmployee" integer NOT NULL,
    "LastName" character varying(100) NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "MiddleName" character varying(100),
    "NumberPhone" character varying(11) NOT NULL,
    "DateBirth" date NOT NULL
);


ALTER TABLE public."Employees" OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 24577)
-- Name: Employees_IdEmployee_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Employees_IdEmployee_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Employees_IdEmployee_seq" OWNER TO postgres;

--
-- TOC entry 5122 (class 0 OID 0)
-- Dependencies: 220
-- Name: Employees_IdEmployee_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Employees_IdEmployee_seq" OWNED BY public."Employees"."IdEmployee";


--
-- TOC entry 238 (class 1259 OID 24718)
-- Name: Events; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Events" (
    "IdEvent" integer NOT NULL,
    "IdOrganizer" integer NOT NULL,
    "EventDescription" text NOT NULL,
    "IdTypeEvent" integer NOT NULL,
    "AvgTicketPrice" numeric NOT NULL,
    "EventDate" timestamp without time zone NOT NULL,
    "EventName" character varying(150) NOT NULL
);


ALTER TABLE public."Events" OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 24715)
-- Name: Events_IdEvent_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Events_IdEvent_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Events_IdEvent_seq" OWNER TO postgres;

--
-- TOC entry 5123 (class 0 OID 0)
-- Dependencies: 235
-- Name: Events_IdEvent_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Events_IdEvent_seq" OWNED BY public."Events"."IdEvent";


--
-- TOC entry 236 (class 1259 OID 24716)
-- Name: Events_IdOrganizer_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Events_IdOrganizer_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Events_IdOrganizer_seq" OWNER TO postgres;

--
-- TOC entry 5124 (class 0 OID 0)
-- Dependencies: 236
-- Name: Events_IdOrganizer_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Events_IdOrganizer_seq" OWNED BY public."Events"."IdOrganizer";


--
-- TOC entry 237 (class 1259 OID 24717)
-- Name: Events_IdTypeEvent_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Events_IdTypeEvent_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Events_IdTypeEvent_seq" OWNER TO postgres;

--
-- TOC entry 5125 (class 0 OID 0)
-- Dependencies: 237
-- Name: Events_IdTypeEvent_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Events_IdTypeEvent_seq" OWNED BY public."Events"."IdTypeEvent";


--
-- TOC entry 227 (class 1259 OID 24643)
-- Name: LogsActions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LogsActions" (
    "IdRecordLogAction" bigint NOT NULL,
    "IdUser" integer,
    "Action" character varying(100) NOT NULL,
    "Object" character varying(100) NOT NULL,
    "ActionDateTime" timestamp without time zone NOT NULL,
    "ActionSystem" character varying(50) NOT NULL
);


ALTER TABLE public."LogsActions" OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 24641)
-- Name: LogsActions_IdRecordLogAction_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."LogsActions_IdRecordLogAction_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."LogsActions_IdRecordLogAction_seq" OWNER TO postgres;

--
-- TOC entry 5126 (class 0 OID 0)
-- Dependencies: 225
-- Name: LogsActions_IdRecordLogAction_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."LogsActions_IdRecordLogAction_seq" OWNED BY public."LogsActions"."IdRecordLogAction";


--
-- TOC entry 226 (class 1259 OID 24642)
-- Name: LogsActions_IdUser_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."LogsActions_IdUser_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."LogsActions_IdUser_seq" OWNER TO postgres;

--
-- TOC entry 5127 (class 0 OID 0)
-- Dependencies: 226
-- Name: LogsActions_IdUser_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."LogsActions_IdUser_seq" OWNED BY public."LogsActions"."IdUser";


--
-- TOC entry 230 (class 1259 OID 24674)
-- Name: LogsErrors; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LogsErrors" (
    "IdRecordLogError" bigint NOT NULL,
    "IdUser" integer,
    "ErrorDescription" text NOT NULL,
    "ErrorDateTime" timestamp without time zone NOT NULL,
    "ErrorSystem" character varying(50) NOT NULL
);


ALTER TABLE public."LogsErrors" OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 24672)
-- Name: LogsErrors_IdRecordLogError_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."LogsErrors_IdRecordLogError_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."LogsErrors_IdRecordLogError_seq" OWNER TO postgres;

--
-- TOC entry 5128 (class 0 OID 0)
-- Dependencies: 228
-- Name: LogsErrors_IdRecordLogError_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."LogsErrors_IdRecordLogError_seq" OWNED BY public."LogsErrors"."IdRecordLogError";


--
-- TOC entry 229 (class 1259 OID 24673)
-- Name: LogsErrors_IdUser_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."LogsErrors_IdUser_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."LogsErrors_IdUser_seq" OWNER TO postgres;

--
-- TOC entry 5129 (class 0 OID 0)
-- Dependencies: 229
-- Name: LogsErrors_IdUser_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."LogsErrors_IdUser_seq" OWNED BY public."LogsErrors"."IdUser";


--
-- TOC entry 232 (class 1259 OID 24694)
-- Name: Organizers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Organizers" (
    "IdOrganizer" integer NOT NULL,
    "OrganizerName" character varying(255) NOT NULL,
    "OrganizerNumberPhone" character varying(11) NOT NULL,
    "OrganizerEmail" character varying(100) NOT NULL,
    "OrganizerDescription" text
);


ALTER TABLE public."Organizers" OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 24693)
-- Name: Organizers_IdOrganizer_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Organizers_IdOrganizer_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Organizers_IdOrganizer_seq" OWNER TO postgres;

--
-- TOC entry 5130 (class 0 OID 0)
-- Dependencies: 231
-- Name: Organizers_IdOrganizer_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Organizers_IdOrganizer_seq" OWNED BY public."Organizers"."IdOrganizer";


--
-- TOC entry 242 (class 1259 OID 24782)
-- Name: Tickets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tickets" (
    "IdTicket" bigint NOT NULL,
    "UnicTicketNumber" character varying(10) NOT NULL,
    "IdEvent" integer NOT NULL,
    "IdUser" integer,
    "Seat" character varying(10) NOT NULL,
    "TicketPrice" numeric NOT NULL,
    "SellDateTime" timestamp without time zone NOT NULL
);


ALTER TABLE public."Tickets" OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 24780)
-- Name: Tickets_IdEvent_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Tickets_IdEvent_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Tickets_IdEvent_seq" OWNER TO postgres;

--
-- TOC entry 5131 (class 0 OID 0)
-- Dependencies: 240
-- Name: Tickets_IdEvent_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Tickets_IdEvent_seq" OWNED BY public."Tickets"."IdEvent";


--
-- TOC entry 239 (class 1259 OID 24779)
-- Name: Tickets_IdTicket_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Tickets_IdTicket_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Tickets_IdTicket_seq" OWNER TO postgres;

--
-- TOC entry 5132 (class 0 OID 0)
-- Dependencies: 239
-- Name: Tickets_IdTicket_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Tickets_IdTicket_seq" OWNED BY public."Tickets"."IdTicket";


--
-- TOC entry 241 (class 1259 OID 24781)
-- Name: Tickets_IdUser_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Tickets_IdUser_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Tickets_IdUser_seq" OWNER TO postgres;

--
-- TOC entry 5133 (class 0 OID 0)
-- Dependencies: 241
-- Name: Tickets_IdUser_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Tickets_IdUser_seq" OWNED BY public."Tickets"."IdUser";


--
-- TOC entry 234 (class 1259 OID 24707)
-- Name: TypesEvents; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TypesEvents" (
    "IdTypeEvent" integer NOT NULL,
    "TypeEvent" character varying(100) NOT NULL
);


ALTER TABLE public."TypesEvents" OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 24706)
-- Name: TypesEvents_IdTypeEvent_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."TypesEvents_IdTypeEvent_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."TypesEvents_IdTypeEvent_seq" OWNER TO postgres;

--
-- TOC entry 5134 (class 0 OID 0)
-- Dependencies: 233
-- Name: TypesEvents_IdTypeEvent_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."TypesEvents_IdTypeEvent_seq" OWNED BY public."TypesEvents"."IdTypeEvent";


--
-- TOC entry 224 (class 1259 OID 24591)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "IdUser" integer NOT NULL,
    "Login" character varying(50) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "Role" character varying(50) NOT NULL,
    "IdEmployee" integer
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 24590)
-- Name: Users_IdEmployee_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Users_IdEmployee_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Users_IdEmployee_seq" OWNER TO postgres;

--
-- TOC entry 5135 (class 0 OID 0)
-- Dependencies: 223
-- Name: Users_IdEmployee_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Users_IdEmployee_seq" OWNED BY public."Users"."IdEmployee";


--
-- TOC entry 222 (class 1259 OID 24589)
-- Name: Users_IdUser_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Users_IdUser_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public."Users_IdUser_seq" OWNER TO postgres;

--
-- TOC entry 5136 (class 0 OID 0)
-- Dependencies: 222
-- Name: Users_IdUser_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Users_IdUser_seq" OWNED BY public."Users"."IdUser";


--
-- TOC entry 4936 (class 2604 OID 24581)
-- Name: Employees IdEmployee; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Employees" ALTER COLUMN "IdEmployee" SET DEFAULT nextval('public."Employees_IdEmployee_seq"'::regclass);


--
-- TOC entry 4942 (class 2604 OID 24721)
-- Name: Events IdEvent; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Events" ALTER COLUMN "IdEvent" SET DEFAULT nextval('public."Events_IdEvent_seq"'::regclass);


--
-- TOC entry 4943 (class 2604 OID 24723)
-- Name: Events IdTypeEvent; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Events" ALTER COLUMN "IdTypeEvent" SET DEFAULT nextval('public."Events_IdTypeEvent_seq"'::regclass);


--
-- TOC entry 4938 (class 2604 OID 24646)
-- Name: LogsActions IdRecordLogAction; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LogsActions" ALTER COLUMN "IdRecordLogAction" SET DEFAULT nextval('public."LogsActions_IdRecordLogAction_seq"'::regclass);


--
-- TOC entry 4939 (class 2604 OID 24677)
-- Name: LogsErrors IdRecordLogError; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LogsErrors" ALTER COLUMN "IdRecordLogError" SET DEFAULT nextval('public."LogsErrors_IdRecordLogError_seq"'::regclass);


--
-- TOC entry 4940 (class 2604 OID 24697)
-- Name: Organizers IdOrganizer; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Organizers" ALTER COLUMN "IdOrganizer" SET DEFAULT nextval('public."Organizers_IdOrganizer_seq"'::regclass);


--
-- TOC entry 4944 (class 2604 OID 24785)
-- Name: Tickets IdTicket; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets" ALTER COLUMN "IdTicket" SET DEFAULT nextval('public."Tickets_IdTicket_seq"'::regclass);


--
-- TOC entry 4941 (class 2604 OID 24710)
-- Name: TypesEvents IdTypeEvent; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TypesEvents" ALTER COLUMN "IdTypeEvent" SET DEFAULT nextval('public."TypesEvents_IdTypeEvent_seq"'::regclass);


--
-- TOC entry 4937 (class 2604 OID 24594)
-- Name: Users IdUser; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users" ALTER COLUMN "IdUser" SET DEFAULT nextval('public."Users_IdUser_seq"'::regclass);


--
-- TOC entry 4946 (class 2606 OID 24588)
-- Name: Employees Employees_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Employees"
    ADD CONSTRAINT "Employees_pkey" PRIMARY KEY ("IdEmployee");


--
-- TOC entry 4959 (class 2606 OID 24733)
-- Name: Events Events_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Events"
    ADD CONSTRAINT "Events_pkey" PRIMARY KEY ("IdEvent");


--
-- TOC entry 4951 (class 2606 OID 24655)
-- Name: LogsActions LogsActions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LogsActions"
    ADD CONSTRAINT "LogsActions_pkey" PRIMARY KEY ("IdRecordLogAction");


--
-- TOC entry 4953 (class 2606 OID 24687)
-- Name: LogsErrors LogsErrors_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LogsErrors"
    ADD CONSTRAINT "LogsErrors_pkey" PRIMARY KEY ("IdRecordLogError");


--
-- TOC entry 4955 (class 2606 OID 24705)
-- Name: Organizers Organizers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Organizers"
    ADD CONSTRAINT "Organizers_pkey" PRIMARY KEY ("IdOrganizer");


--
-- TOC entry 4961 (class 2606 OID 24798)
-- Name: Tickets Tickets_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets"
    ADD CONSTRAINT "Tickets_pkey" PRIMARY KEY ("IdTicket");


--
-- TOC entry 4957 (class 2606 OID 24714)
-- Name: TypesEvents TypesEvents_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TypesEvents"
    ADD CONSTRAINT "TypesEvents_pkey" PRIMARY KEY ("IdTypeEvent");


--
-- TOC entry 4949 (class 2606 OID 24602)
-- Name: Users Users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("IdUser");


--
-- TOC entry 4947 (class 1259 OID 24666)
-- Name: Users_IdEmployee_fkey; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "Users_IdEmployee_fkey" ON public."Users" USING btree ("IdEmployee");


--
-- TOC entry 4965 (class 2606 OID 24734)
-- Name: Events Events_IdOrganizer_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Events"
    ADD CONSTRAINT "Events_IdOrganizer_fkey" FOREIGN KEY ("IdOrganizer") REFERENCES public."Organizers"("IdOrganizer");


--
-- TOC entry 4966 (class 2606 OID 40960)
-- Name: Events Events_IdTypeEvent_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Events"
    ADD CONSTRAINT "Events_IdTypeEvent_fkey" FOREIGN KEY ("IdTypeEvent") REFERENCES public."TypesEvents"("IdTypeEvent");


--
-- TOC entry 4963 (class 2606 OID 49152)
-- Name: LogsActions LogsActions_IdUser_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LogsActions"
    ADD CONSTRAINT "LogsActions_IdUser_fkey" FOREIGN KEY ("IdUser") REFERENCES public."Users"("IdUser") ON DELETE SET NULL;


--
-- TOC entry 4964 (class 2606 OID 49157)
-- Name: LogsErrors LogsErrors_IdUser_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LogsErrors"
    ADD CONSTRAINT "LogsErrors_IdUser_fkey" FOREIGN KEY ("IdUser") REFERENCES public."Users"("IdUser") ON DELETE SET NULL;


--
-- TOC entry 4967 (class 2606 OID 24799)
-- Name: Tickets Tickets_IdEvent_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets"
    ADD CONSTRAINT "Tickets_IdEvent_fkey" FOREIGN KEY ("IdEvent") REFERENCES public."Events"("IdEvent");


--
-- TOC entry 4968 (class 2606 OID 24804)
-- Name: Tickets Tickets_IdUser_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tickets"
    ADD CONSTRAINT "Tickets_IdUser_fkey" FOREIGN KEY ("IdUser") REFERENCES public."Users"("IdUser");


--
-- TOC entry 4962 (class 2606 OID 49162)
-- Name: Users Users_IdEmployee_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_IdEmployee_fkey" FOREIGN KEY ("IdEmployee") REFERENCES public."Employees"("IdEmployee") ON DELETE SET NULL;



--
-- PostgreSQL database dump complete
--

\unrestrict khruYodPIZqHuj4sauabg09MaLVAv8R0Xr9q9noD627YNluscEflbNdDMVheK95

