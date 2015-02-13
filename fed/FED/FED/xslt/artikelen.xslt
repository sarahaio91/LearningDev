<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:variable name="title" select="hf/@title" />

  <xsl:template match="parametergrp | kenmerkgrp | dossierredromp" />

  <xsl:template
    match="ue | hfpart | commentaarcontent | p | geenverwijzing | jurpubcontent | dossierred | essentiekern | uitspraakred | uitspraakredromp | uitspraaktekst | annotatie | annotatiekop | uitspraaksam">
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
        <div class="{substring(ue/parametergrp/parameter[@naam = 'type'], 4)}">
          <div class="kopgeghftekst">
            <a href="{concat('../Text/inhoud.html#back_', generate-id(ue/hftekst[@hftekstinhoud = 'kopgeg']/kopgeg/titel))}">
              <span class="titel" id="{generate-id(ue/hftekst[@hftekstinhoud = 'kopgeg']/kopgeg/titel)}">
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

  <xsl:template match="hftekst[@hftekstinhoud='auteurgeg']">
    <div class="ablok">
      <div class="auteurgeg">
        <b>
          <xsl:apply-templates />
        </b>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="pe">
    <xsl:apply-templates />
    <xsl:if test="current()//noot">
      <hr class="calibre5" />
      <xsl:call-template name="showNotes">
        <xsl:with-param name="pe" select="current()" />
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="showNotes">
    <xsl:param name="pe" select="node()" />
    <table class="calibre1">
      <xsl:for-each select="$pe//noot">
        <xsl:variable name="noteNum" select="count(preceding::noot[ancestor::pe = current()/ancestor::pe])+1" />
        <tr class="calibre2">
          <td class="kopnritem">
            <a class="titel1" href="{concat('#', 'footnote_', generate-id())}"
               id="{concat('back_footnote_', generate-id())}">
              <xsl:value-of select="$noteNum" />
            </a>
          </td>
          <td class="kopnritem">
            <xsl:apply-templates select="a" />
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template match="verhandelingalgemeen">
    <xsl:apply-templates select="auteurgeg" />

    <div class="kopgegverhandelingalgemeen" id="{generate-id(kopgeg/*/text())}">
      <span class="titel">
        <xsl:value-of select="kopgeg/*/text()" />
      </span>
    </div>
    <div class="ablok">
      <a
        href="{concat('../Text/inhoud.html#back_', generate-id(../../kenmerkgrp/kenmerk[@naam = 'vindplaats.publicatienummer']))}"
        id="{generate-id(../../kenmerkgrp/kenmerk[@naam = 'vindplaats.publicatienummer'])}" class="vindplaats1">
        <xsl:value-of select="../../kenmerkgrp/kenmerk[@naam = 'vindplaats.publicatienummer']/text()" />
      </a>
    </div>

    <xsl:apply-templates select="samenvatting" />
    <xsl:apply-templates select="child::*[not(name() = 'auteurgeg' or name() = 'kopgeg' or name() = 'samenvatting')]" />
  </xsl:template>

  <xsl:template match="samenvatting | citaat">
    <div class="{name()}">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="auteurgeg">
    <div class="auteurgeg">
      <b>
        <xsl:apply-templates />
      </b>
    </div>
  </xsl:template>

  <xsl:template match="noot">
    <xsl:variable name="noteNum" select="count(preceding::noot[ancestor::pe = current()/ancestor::pe])+1" />
    <span class="super">
      <a href="{concat('#back_footnote_', generate-id())}" id="{concat('footnote_' , generate-id())}" class="ixlink">
        <xsl:value-of select="$noteNum" />
      </a>
    </span>
  </xsl:template>

  <xsl:template match="noot/a">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="p/kopgeg[not(parent::*[ancestor::*[local-name() = 'p']])]">
    <div class="kopgegp">
      <span class="kopnr">
        <xsl:value-of select="current()/kopnr/text()" />
      </span>
      <xsl:text>&#160;</xsl:text>
      <xsl:choose>
        <xsl:when test="string-length(current()/kopnr/text())">
          <span class="titel1">
            <xsl:value-of select="current()/titel/text()" />
          </span>
        </xsl:when>
        <xsl:otherwise>
          <span class="titel1" style="margin-left: 1.5em;">
            <xsl:value-of select="current()/titel/text()" />
          </span>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="p/p/kopgeg[not(parent::*[parent::*[ancestor::*[local-name() = 'p']]])]">
    <div class="kopgegp1">
      <span class="kopnr">
        <xsl:value-of select="current()/kopnr/text()" />
      </span>
      <xsl:text>&#160;</xsl:text>
      <xsl:choose>
        <xsl:when test="string-length(current()/kopnr/text())">
          <span class="titel1">
            <xsl:value-of select="current()/titel/text()" />
          </span>
        </xsl:when>
        <xsl:otherwise>
          <span class="titel1" style="margin-left: 1.5em;">
            <xsl:value-of select="current()/titel/text()" />
          </span>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="p/p/p/kopgeg[not(parent::*[parent::*[parent::*[ancestor::*[local-name() = 'p']]]])] ">
    <div class="kopgegp2">
      <span class="kopnr">
        <xsl:value-of select="current()/kopnr/text()" />
      </span>
      <xsl:text>&#160;</xsl:text>
      <xsl:choose>
        <xsl:when test="string-length(current()/kopnr/text())">
          <span class="titel1">
            <xsl:value-of select="current()/titel/text()" />
          </span>
          
        </xsl:when>
        <xsl:otherwise>
          <span class="titel1" style="margin-left: 1.5em;">
            <xsl:value-of select="current()/titel/text()" />
          </span>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="ablok/kopgeg">
    <div class="kopgegp2">
      <xsl:if test="titel">
        <xsl:if test="string-length(kopnr/text())">
          <span class="kopnr">
            <xsl:value-of select="current()/kopnr/text()" />
          </span>
          <xsl:text>&#160;</xsl:text>
        </xsl:if>    
      </xsl:if>
      <xsl:choose>
        <xsl:when test ="titel">
          <span class="titel1">
            <xsl:value-of select="current()/titel/text()" />
          </span>  
        </xsl:when> 
        <xsl:otherwise>
          <span class="prefixAblok">
            <xsl:value-of select="concat(current()/kopnr/text(), ' ')" />
          </span>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

  <xsl:template match="ablok">
    <div class="{name()}">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="ablok/a">
    <xsl:choose>
      <xsl:when test="text()!=''">
        <span class="ablok">
          <xsl:apply-templates />
        </span>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="verwijzing">
    <a href="{@href}" class="toclink">
      <xsl:apply-templates />
    </a>
  </xsl:template>

  <xsl:template match="nadruk">
    <span class="{@opmaak}">
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

  <xsl:template match="table">
    <img alt="" class="tableimage" src="{concat('../Images/',@id,'.png')}" />
  </xsl:template>

  <xsl:template match="super">
    <span class="super">
      <xsl:apply-templates />
    </span>
  </xsl:template>

  <xsl:template match="afb">
    <img class="tableimage" style="width:350px;"
         src="{concat('../Images/', substring-before(current()/afbref/@afb-naam, '.tif'), '.png')}">
    </img>
  </xsl:template>

  <xsl:template match="dossierredkop">
    <div class="vindplaats">
      <span class="vindplaats">
        <xsl:value-of select="current()/uitspraakgeg/vindplaats/text()" />
      </span>
    </div>
    <div class="instantienaam">
      <span class="instantienaam">
        <xsl:value-of select="current()/uitspraakgeg/instantie/instantienaam/text()" />
      </span>
    </div>
      <xsl:if test="current()/uitspraakgeg/datum-uitspraak and current()/uitspraakgeg/zaaknr">
        <div class="ablok">
          <span class="uitspraakgeg">
            <xsl:value-of
              select="concat(current()/uitspraakgeg/datum-uitspraak/text(), '&#160;nr.&#160;', current()/uitspraakgeg/zaaknr/text())" />
          </span>
        </div>
      </xsl:if>
      <xsl:if test="current()/rechtergeg and current()/conclusienemer">
        <div class="ablok">
          <span class="rechtergeg">
            <xsl:value-of
              select="concat('(', current()/rechtergeg/text(), ';', current()/conclusienemer/text(), ')')" />
          </span>
        </div>
      </xsl:if>
      <xsl:if test="current()/rechtergeg and not(current()/conclusienemer)">
        <div class="ablok">
          <span class="rechtergeg">
            <xsl:value-of
              select="concat('(', current()/rechtergeg/text(), ')')" />
          </span>
        </div>
      </xsl:if>
      <xsl:if test="current()/annotator">
        <div class="ablok">
          <span class="annotator">
            <xsl:value-of
              select="concat('m.nt.&#160;', current()/annotator/text())" />
          </span>
        </div>
      </xsl:if>
      <xsl:if test="current()/wetingang">
        <div class="ablok">
          <span class="wetingang">
            <xsl:value-of
              select="current()/wetingang/text()" />
          </span>
        </div>
      </xsl:if>
    
    
    <xsl:for-each select="current()/vindplaatsred/a">
      <div class="ablok">
        <span class="vindplaatsred">
          <xsl:value-of select="." />
        </span>
      </div>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="essentie">
    <span class="essentiekern">
      <b>
        <xsl:apply-templates select="essentiekern" />
      </b>
    </span>
  </xsl:template>

  <xsl:template match="a[@type = 'pageref']">
    <span class="pagerefdiv">
      <a class="pcalibre pageref pcalibre1" href="artikelen.html#pageindex_{text()}" id="pageref_{text()}">
        <xsl:value-of select="node()" />
      </a>
    </span>
  </xsl:template>
  
  <xsl:template match="annotatie/annotatieromp">
    <div class="balk">
      <div class="annotatienoot">Noot</div>
    </div>
    <xsl:apply-templates/>
  </xsl:template>
  
  <xsl:template match="annotatiekop/annotator">
    <p class="annotator">
      <xsl:apply-templates/>
    </p>
  </xsl:template>
  
  <xsl:template match="uitspraaksam/uitspraaksamromp">
    <div class="uitspraaksamromp">
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>
