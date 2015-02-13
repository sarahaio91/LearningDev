<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:variable name="title" select="hf/@title" />

  <xsl:template match="parametergrp | kenmerkgrp | a[text() = ' ' ] | verhandelingalgemeen/kopgeg"/>

  <xsl:template match="ue | hfpart | geenverwijzing | nadruk[count(@*) = 0] | verhandelingalgemeen | ablok ">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="nadruk[@opmaak='vet']">
    <b>
      <xsl:apply-templates/>
    </b>
  </xsl:template>

  <xsl:template match="nadruk[@opmaak='cursief']">
    <xsl:choose>
      <xsl:when test="string-length(text()) = 0">
      </xsl:when>
      <xsl:otherwise>
        <i>
          <xsl:apply-templates/>
        </i>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="nadruk[@opmaak='onderstreept']">
    <span class="{@opmaak}">
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match="hf">
    <html>
      <head>
        <title>
          <xsl:value-of select="$title" />
        </title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="../Styles/stylesheet.css" type="text/css" rel="stylesheet" />
      </head>
      <body class="calibre">
        <xsl:apply-templates select="node()" />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="hfpart[@id]">
    <xsl:choose>
      <xsl:when test="not(descendant::hfpart)">
        <div class="pe">
          <p class="kopgeghftekst" id="{generate-id()}">
            <a href="{concat('inhoud.html#back', generate-id())}">
              <xsl:value-of select="ue/hftekst[@hftekstinhoud = 'kopgeg']/kopgeg/kopnaam" />
            </a>
          </p>
          <xsl:apply-templates select="pe" />
        </div>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="ue/hfpart" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="pe">

    <xsl:choose>
      <xsl:when test="commentaarcontent">
        <xsl:apply-templates/>
      </xsl:when>
      <xsl:otherwise>
        <p class="ch-num" id="{generate-id(kenmerkgrp[1])}">
          <a href="{concat('inhoud.html#back', generate-id(kenmerkgrp[1]))}">
            <xsl:value-of select="kenmerkgrp[1]/kenmerk[1]" />
          </a>
        </p>
        <xsl:apply-templates select="child::*" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match ="jurpubcontent">
    <xsl:choose>
      <xsl:when test="essentie/essentiekern/child::*[name() = 'nadruk']">
        <p class="ch-title" id="{generate-id(essentie[1])}">
          <xsl:if test="essentie/essentiekern/verwijzing">
            <xsl:apply-templates select="essentie/essentiekern/verwijzing"/>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="essentie/essentiekern/nadruk">
              <xsl:value-of select="essentie/essentiekern/nadruk/text()"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="essentie/essentiekern/text()"/>
            </xsl:otherwise>
          </xsl:choose>
        </p>
      </xsl:when>
      <xsl:otherwise>
        <p class="ch-title" id="{generate-id(essentie[1])}">
          <xsl:if test="essentie/essentiekern/verwijzing">
            <xsl:apply-templates select="essentie/essentiekern/verwijzing"/>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="essentie/essentiekern/nadruk">
              <xsl:value-of select="essentie/essentiekern/nadruk/text()"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="essentie/essentiekern/text()"/>
            </xsl:otherwise>
          </xsl:choose>
        </p>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:apply-templates select="dossierred"/>
    <xsl:apply-templates select="essentie"/>
    <xsl:apply-templates select="uitspraaksam"/>
    <xsl:apply-templates select="verwantoordeel"/>
    <xsl:apply-templates select="praktijktip"/>
  </xsl:template>

  <xsl:template match="commentaarcontent">
    <p class="ch-title1" id="{generate-id()}">
      <xsl:value-of select="verhandelingalgemeen/kopgeg/titel"/>
    </p>
    <p class="ch-num1" id="{generate-id(parent::*[name() = 'pe']/kenmerkgrp[1]/kenmerk[1])}">
      <a href="{concat('inhoud.html#back', generate-id(parent::*[name() = 'pe']/kenmerkgrp[1]/kenmerk[1]))}">
        <xsl:value-of select="parent::*[name() = 'pe']/kenmerkgrp[1]/kenmerk[1]"/>
      </a>
    </p>
    <xsl:apply-templates select="child::*" />
  </xsl:template>
  
  <xsl:template match="dossierred">
    <p class="noindent">
      <xsl:value-of select="dossierredkop/uitspraakgeg/instantie/instantienaam"/>
      <xsl:text>&#160;</xsl:text>
      <xsl:value-of select="dossierredkop/uitspraakgeg/datum-uitspraak"/>
      <xsl:text>,&#160;nr.&#160;</xsl:text>
      <xsl:value-of select="dossierredkop/uitspraakgeg/zaaknr"/>
    </p>
    <p class="noindent">
      <xsl:value-of select="dossierredkop/wetingang"/>
    </p>
    <xsl:for-each select="dossierredkop/vindplaatsred/a">
      <xsl:choose>
        <xsl:when test="position() &gt; 1">
          <p class="noindent1">
            <xsl:value-of select="."/>
          </p>
        </xsl:when>
        <xsl:otherwise>
          <p class="noindent">
            <xsl:value-of select="."/>
          </p>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="essentie">
    <p class="noindent">
      <b>
        <xsl:apply-templates select="essentieromp/node()"/>
      </b>
    </p>
  </xsl:template>

  <xsl:template match="uitspraaksam">
    <xsl:for-each select="uitspraaksamromp/ablok">
      <xsl:apply-templates />
    </xsl:for-each>
  </xsl:template>


  <xsl:template match="verwijzing">
    <a href="{@href}" id="{@id}">
      <xsl:apply-templates />
    </a>
  </xsl:template>

  <xsl:template match="a">
    <xsl:choose>
      <xsl:when test="preceding-sibling::*[name() = 'a']">
        <p class="noindent1">
          <xsl:apply-templates/>
        </p>
      </xsl:when>
      <xsl:otherwise>
        <p class="noindent">
          <xsl:apply-templates/>
        </p>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="verwantoordeel">
    <p class="noindent">
      <b>
        <xsl:value-of select="verwantoordeelromp/idemoordeel/kopgeg/titel"/>
      </b>
    </p>
    <xsl:apply-templates select="verwantoordeelromp/idemoordeel/idemoordeelromp/node()"/>
  </xsl:template>

  <xsl:template match="lijst">
    <xsl:for-each select="item">
      <p class="list">
        <xsl:value-of select="kopgeg/kopnr"/>
        <xsl:text>&#160;&#160;&#160;&#160;&#160;&#160;</xsl:text>
        <xsl:value-of select="a"/>
      </p>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="praktijktip">
    <div class="box">
      <p class="noindent1">
        <b>
          <xsl:value-of select="kopgeg/titel"/>
        </b>
      </p>
      <xsl:apply-templates select="praktijktipromp/node()"/>
    </div>
  </xsl:template>

  <xsl:template match="commentaarcontent/verhandelingalgemeen/ablok[preceding-sibling::*[name() = 'ablok']][last()]">
    <div class="box">
      <p class="noindent1">
        <b>
          <xsl:value-of select="kopgeg/ondertitel/nadruk"/>
        </b>
      </p>
      <xsl:if test="kopgeg/titel">
        <p class="h1">
          <xsl:value-of select="kopgeg/titel"/>
        </p>
      </xsl:if>
      <xsl:apply-templates select="node()[position() &gt; 1]"/>
    </div>
  </xsl:template>

  <xsl:template match="commentaarcontent/verhandelingalgemeen/p">
    <xsl:choose>
      <xsl:when test="p">
        <p class="h1">
          <xsl:value-of select="kopgeg/titel"/>
        </p>
      </xsl:when>
      <xsl:otherwise>
        <p class="h2">
          <xsl:value-of select="kopgeg/titel"/>
        </p>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:apply-templates select="ablok/node()"/>
    <xsl:apply-templates select="p/node()"/>
  </xsl:template>

  <xsl:template match="commentaarcontent/verhandelingalgemeen/p/p">
    <xsl:apply-templates select="kopgeg"/>
    <xsl:apply-templates select="ablok/node()"/>
  </xsl:template>

  <xsl:template match="kopgeg">
    <xsl:choose>
      <xsl:when test="count(descendant::nadruk) = 0">
        <p class="h1">
          <xsl:value-of select="titel/node()"/>
        </p>
      </xsl:when>
      <xsl:otherwise>
        <p class="h2">
          <xsl:value-of select="titel/node()"/>
        </p>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="p/p/kopgeg">
    <xsl:choose>
      <xsl:when test="count(descendant::nadruk) = 0">
        <p class="h1">
          <xsl:value-of select="titel/node()"/>
        </p>
      </xsl:when>
      <xsl:otherwise>
        <p class="h2">
          <xsl:value-of select="titel/node()"/>
        </p>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="table[@id]">
    <img alt="" class="tableimage" src="{concat(concat('../Images/', @id), '.png')}"/>
  </xsl:template>

  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>