<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="hf">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>Belastingblad 7</title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="stylesheet.css" rel="stylesheet" type="text/css" />
        <link href="page_styles.css" rel="stylesheet" type="text/css" />
      </head>
      <body class="calibre">
        <xsl:apply-templates />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="parametergrp | kenmerkgrp | hftekst[@hftekstinhoud = 'auteurgeg']" />

  <xsl:template
    match="ue | hfpart | commentaarcontent | verhandelingalgemeen | p | titel | jurpubcontent | uitspraakgeg | dossierredkop | instantie | vindplaats | instantienaam | datum-uitspraak | zaaknr | geenverwijzing | uitspraaktekst">
    <xsl:apply-templates />
  </xsl:template>

  <xsl:template match="hftekst[@hftekstinhoud='kopgeg']">
    <xsl:variable name="id" select="generate-id()" />
    <div class="hftekst">
      <div class="balk">
        <h2 class="kopgeghftekst">
          <span class="hfteksttitel">
            <a href="{concat('../Text/Inhoud.html#back_', $id)}" id="{$id}" class="hfteksttitel">
              <xsl:value-of select="current()/kopgeg/titel/text()" />
            </a>
          </span>
        </h2>
      </div>
    </div>
  </xsl:template>

  <xsl:template match="pe">
    <div class="hftekst">
      <xsl:apply-templates />
      <xsl:if test="current()//noot">
        <hr class="calibre6" />
        <xsl:call-template name="showNotes">
          <xsl:with-param name="pe" select="current()" />
        </xsl:call-template>
      </xsl:if>
    </div>
  </xsl:template>

  <xsl:template match="pe/kenmerkgrp">
    <xsl:variable name="id" select="generate-id()" />
    <xsl:variable name="temp" select="current()/following-sibling::jurpubcontent" />
    <xsl:choose>
      <xsl:when test="current()/following-sibling::jurpubcontent">
        <xsl:apply-templates select="preceding-sibling::jurpubcontent" />
      </xsl:when>
      <xsl:otherwise>
        <div class="hftekst">
          <a href="{concat('../Text/Inhoud.html#back_', $id)}" id="{$id}" class="vindplaats1">
            <xsl:value-of select="current()/kenmerk[@naam='vindplaats.publicatienummer']/text()" />
          </a>
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="kopgeg">
    <xsl:variable name="kopnr" select="current()/kopnr" />
    <xsl:variable name="titel" select="current()/titel" />
    <xsl:variable name="kopnaam" select="current()/kopnaam" />
    <xsl:choose>
      <xsl:when test="current()/ancestor::verhandelingalgemeen">
        <xsl:choose>
          <xsl:when test="string-length($titel) and string-length($kopnr) = 0">
            <h3 class="h">
              <span class="hfteksttitel">
                <xsl:apply-templates select="current()/titel" />
              </span>
            </h3>
          </xsl:when>
          <xsl:when test="string-length($titel) and string-length($kopnr)">
            <div class="kopgegp">
              <span class="kopnrp">
                <xsl:value-of select="$kopnr" />
              </span>
              <span class="toclink">
                <xsl:apply-templates select="current()/titel" />
              </span>
            </div>
          </xsl:when>
        </xsl:choose>
      </xsl:when>

      <xsl:when test="current()/ancestor::citaat">
        <div class="kopgegp2">
          <xsl:if test="string-length($titel)">
            <span class="toclink">
              <xsl:apply-templates select="current()/titel" />
            </span>
          </xsl:if>

          <xsl:if test="string-length($kopnaam)">
            <span class="toclink kopnaam">
              <xsl:apply-templates select="current()/kopnaam/node()" />
            </span>
          </xsl:if>
          <xsl:if test="string-length($kopnr)">
            <span class="kopnrp">
              <xsl:apply-templates select="current()/kopnr/node()" />
            </span>
          </xsl:if>
        </div>
      </xsl:when>

      <xsl:when test="current()/ancestor::uitspraaktekst or current()/ancestor::annotatie">
        <xsl:choose>
          <xsl:when test="string-length($titel) and string-length($kopnr)">
            <div class="kopgegp1">
              <span class="kopnrp">
                <xsl:value-of select="$kopnr" />
              </span>
              <span class="toclink">
                <xsl:apply-templates select="current()/titel" />
              </span>
            </div>
          </xsl:when>
          <xsl:when test="string-length($titel) and string-length($kopnr) = 0">
            <div class="kopgegp">
              <span class="toclink">
                <xsl:apply-templates select="current()/titel" />
              </span>
            </div>
          </xsl:when>
          <xsl:when test="string-length($kopnr) and string-length($titel) = 0">
            <div class="kopgegablokkopnr">
              <span class="toclink">
                <xsl:value-of select="$kopnr" />
              </span>
            </div>
          </xsl:when>
        </xsl:choose>
      </xsl:when>

    </xsl:choose>
  </xsl:template>

  <xsl:template match="auteurgeg">
    <div class="auteurgeg">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="ablok">
    <div class="auteurgeg">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="a">
    <xsl:variable name="pos" select="count(preceding::a[parent::ablok = current()/parent::ablok])" />
 
    <xsl:choose>
      <xsl:when test="$pos &gt; 1">
        <div class="hftekst">
          <xsl:text>&#160;&#160;&#160;&#160;</xsl:text>
          <xsl:apply-templates />
        </div>
      </xsl:when>
      <xsl:otherwise>
        <div class="hftekst">
          <xsl:apply-templates />
        </div>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="essentie | uitspraakred| uitspraakredromp | annotatieromp | annotatiekop">
    <div class="hftekst">
      <xsl:apply-templates />
    </div>
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

  <xsl:template match="noot">
    <xsl:variable name="noteNum" select="count(preceding::noot[ancestor::pe = current()/ancestor::pe])+1" />
    <span class="toclink">
      <sup class="calibre5">
        <a href="{concat('#back_footnote_', generate-id())}" id="{concat('footnote_' , generate-id())}" class="toclink">
          <xsl:value-of select="concat('[',$noteNum, ']')" />
        </a>
      </sup>
    </span>
  </xsl:template>

  <xsl:template name="showNotes">
    <xsl:param name="pe" select="node()" />
    <table class="calibre1">
      <xsl:for-each select="$pe//noot">
        <xsl:variable name="noteNum" select="count(preceding::noot[ancestor::pe = current()/ancestor::pe])+1" />
        <tr class="calibre2">
          <td class="vnreflink">
            <a class="toclink" href="{concat('#', 'footnote_', generate-id())}"
               id="{concat('back_footnote_', generate-id())}">
              <xsl:value-of select="$noteNum" />
            </a>
          </td>
          <td class="vnrefcontent">
            <xsl:apply-templates />
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template match="citaat">
    <div class="citaat">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="lijst">
    <xsl:for-each select="current()/item">
      <div class="item">
        <div class="itemkopnr">
          <xsl:apply-templates select="current()/kopgeg/kopnr/node()" />
        </div>
        <div class="itema">
          <xsl:apply-templates select="a" />
        </div>
      </div>
      <xsl:apply-templates select="node()[not(local-name() = 'kopgeg' or local-name() = 'a')]" />
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="dossierred">
    <div class="dossierred">
      <div class="hftekst">
        <a href="{concat('#back_', generate-id())}" id="{generate-id()}" class="vindplaats1">
          <xsl:apply-templates select="current()//vindplaats" />
        </a>
      </div>
      <div class="instantienaam">
        <xsl:apply-templates select="current()//instantienaam" />
      </div>
      <div class="hftekst">
        <xsl:apply-templates select="current()//datum-uitspraak" />
        <xsl:text>, nr. </xsl:text>
        <xsl:apply-templates select="current()//zaaknr" />
      </div>

      <div class="hftekst">
        <xsl:text>(</xsl:text>
        <xsl:apply-templates select="current()//rechtergeg" />
        <xsl:text>)</xsl:text>
      </div>
      <xsl:text> m.nt. </xsl:text>

      <xsl:apply-templates select="current()//annotator" />

      <div class="hftekst">
        <xsl:apply-templates select="current()//geenverwijzing" />
      </div>
    </div>
  </xsl:template>

  <xsl:template match="rechtergeg | annotator">
    <div class="rechtergeg">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="essentiekern">
    <div class="essentiekern">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="essentieromp">
    <div class="essentieromp">
      <xsl:apply-templates />
    </div>
  </xsl:template>

  <xsl:template match="annotatie">
    <div class="balk">
      <div class="annotatienoot"> Noot </div>
    </div>
    <div class="hftekst">
      <xsl:apply-templates />
    </div>
  </xsl:template>
  
  <xsl:template match="table">
   <img class="tableimage" src="{concat('../Images/', current()/@id, '.png')}"></img>
  </xsl:template>

  <xsl:template match="text()[not(normalize-space(.))]" />

</xsl:stylesheet>
