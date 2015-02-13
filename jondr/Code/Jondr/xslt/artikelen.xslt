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

  <xsl:template match="parametergrp | kenmerkgrp | dossierredromp" />

  <xsl:template match="ue | hfpart | commentaarcontent | jurpubcontent | dossierred | uitspraakred">
    <xsl:apply-templates />
  </xsl:template>
  
  <xsl:template match="hf">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>
          <xsl:value-of select="$title" />
        </title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="../Styles/stylesheet.css" type="text/css" rel="stylesheet" />
        <link href="../Styles/page_styles.css" type="text/css" rel="stylesheet" />
      </head>
      <body class="calibre">
        <xsl:apply-templates select="node()" />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="hfpart[@id]">
    <xsl:choose>
      <xsl:when test="not(descendant::hfpart)">
        <!--<xsl:variable name="id" select="generate-id(current()/ue/hftekst[@hftekstinhoud = 'kopgeg']/kopgeg/titel)" />-->
        <xsl:variable name="id" select="generate-id()" />
        <div class="{substring(ue/parametergrp/parameter[@naam = 'type'], 4)}">
          <div class="kopgeghftekst">
            <a
              href="{concat('../Text/inhoud.xhtml#back_', $id)}" id="{$id}" class="calibre4">
              <span class="calibre4">
                <xsl:value-of select="ue/hftekst[@hftekstinhoud = 'kopgeg']/kopgeg/titel" />
              </span>
            </a>
          </div>
        </div>
        <xsl:apply-templates select="ue/hftekst[@hftekstinhoud='auteurgeg']" />
        <xsl:apply-templates select="pe" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="ue/hfpart" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="nieuwsitem">
    <div class="vindplaats" id="{generate-id(../../kenmerkgrp/kenmerk[@naam = 'vindplaats.publicatienummer'])}">
      <xsl:value-of select="../../kenmerkgrp/kenmerk[@naam = 'vindplaats.publicatienummer']/text()" />
    </div>

    <xsl:if test="string-length(current()/kopgeg/titel/text())">
      <div class="kopgegnieuwsitem">
        <xsl:apply-templates select="current()/kopgeg" />
      </div>
    </xsl:if>

    <xsl:apply-templates select="child::*[not(name() = 'kopgeg')]" />
  </xsl:template>

  <xsl:template match="nieuwsitem/kopgeg">
    <xsl:if test="string-length(current()/titel/text())">
      <span class="titel">
        <xsl:apply-templates select="current()/titel/node()" />
      </span>
    </xsl:if>
  </xsl:template>

  <xsl:template match="pe">
    <div class="inhoudsopgave">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="ablok">
    <div class="inhoudsopgave">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="a">
    <span class="inhoudsopgave">
      <xsl:apply-templates />
    </span>
  </xsl:template>

  <xsl:template match="nadruk[@opmaak='cursief']">
    <span class="cursief">
      <xsl:apply-templates />
    </span>
  </xsl:template>

  <xsl:template match="nadruk[@opmaak='vet']">
    <span class="ixlink">
      <xsl:apply-templates />
    </span>
  </xsl:template>

  <xsl:template match="nadruk[@opmaak='vetcursief']">
    <span class="ixlink cursief">
      <xsl:apply-templates />
    </span>
  </xsl:template>

  <xsl:template match="lijst">
    <table class="calibre1">
      <xsl:apply-templates />
    </table>
  </xsl:template>

  <xsl:template match="item">
    <tr class="calibre2">
      <xsl:apply-templates />
    </tr>
  </xsl:template>

  <xsl:template match="item/kopgeg">
    <td class="kopnritem">
      <xsl:apply-templates select="current()/kopnr/text()" />
    </td>
  </xsl:template>

  <xsl:template match="item/a">
    <td class="kopnritem">
      <span class="kopgegitem"></span>
      <span class="titel1">
        <xsl:apply-templates />
      </span>
    </td>
  </xsl:template>

  <xsl:template match="verwijzing">
    <a href="{@href}" class="toclink">
      <xsl:apply-templates />
    </a>
  </xsl:template>

  <xsl:template match="dossierredkop/vindplaatsred/a/verwijzing">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="dossierredkop">
    <div class="inhoudsopgave">
      <div class="vindplaats" id="{generate-id(../../../kenmerkgrp/kenmerk[@naam = 'vindplaats.publicatienummer'])}">
        <xsl:value-of select="../../../kenmerkgrp/kenmerk[@naam = 'vindplaats.publicatienummer']/text()" />
      </div>
      <div class="inhoudsopgave">
        <xsl:value-of select="current()/uitspraakgeg/instantie/instantienaam/text()" />
      </div>
      <div class="datum-uitspraakzaaknr">
        <xsl:value-of
          select="concat(current()/uitspraakgeg/datum-uitspraak/text(), '&#160;nr.&#160;', current()/uitspraakgeg/zaaknr/text())" />
      </div>
      <div class="datum-uitspraakzaaknr">
        <xsl:value-of select="current()/wetingang/text()" />
      </div>
      <div class="datum-uitspraakzaaknr">
        <xsl:apply-templates select="current()/vindplaatsred" />

      </div>
    </div>
  </xsl:template>

  <xsl:template match="vindplaatsred">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="essentie">
    <div class="inhoudsopgave">
      <xsl:apply-templates select="essentieromp" />
    </div>
  </xsl:template>

  <xsl:template match="essentieromp">
    <div class="essentieromp">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="uitspraakredromp">
    <div class="uitspraakredromp">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="uitspraaktekst">
    <div class="inhoudsopgave">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>
