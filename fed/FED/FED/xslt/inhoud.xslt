<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" />

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:variable name="title" select="body/@title" />

  <xsl:template match="body">
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
        <div class="hftekst">
          <div class="kopgeginhoud" id="calibre_toc_1">
            <span class="titel">Inhoud</span>
          </div>
          <table class="calibre1">
            <xsl:apply-templates />
          </table>
          <table class="calibre1">
            <tr class="calibre2">
              <td class="ixverhandelingalgemeen">
                <a href="../Text/artikelen.html#toclink_paginaregister" id="backlink_paginaregister" class="ixlink">Paginaregister</a>
              </td>
            </tr>
          </table>
          <div class="ablok">
            <h3 class="vindplaats" id="calibre_toc_2">Colofon</h3>
            <img src="../Images/colofon.jpg" alt="" class="calibre3" />
          </div>
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="div[not(@class='hftekst') or not(@class='kopgegverhandelingalgemeen')] | hr | table | img | span" />

  <xsl:template match="div[@class='hftekst']">
    <tr class="calibre2">
      <td class="ixhftekst">
        <span class="titel1">
          <xsl:value-of select="." />
        </span>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="div[@class='kopgegverhandelingalgemeen']">
    <xsl:variable name="next" select="current()/following-sibling::*[1]" />
    <xsl:variable name="prev" select="current()/preceding-sibling::*[1]" />
    <tr class="calibre2">
      <td class="ixverhandelingalgemeen">
        <a href="{concat('../Text/artikelen.html#', $next/a/@id)}" id="{concat('back_', $next/a/@id)}" class="ixlink">
          <span class="ixlink">
            <xsl:value-of select="$next" />
          </span>
          <span class="ixlink">
            <xsl:value-of select="." />
          </span>
        </a>
        
        <xsl:variable name="samenvatting" select="current()/following-sibling::*[local-name() = 'div' and @class='samenvatting'][1]"/>
        <xsl:variable name="prevkopgegverhandelingalgemeen" select="$samenvatting/preceding-sibling::*[local-name() = 'div' and @class='kopgegverhandelingalgemeen'][1]"/>
        <xsl:if test="generate-id() = generate-id($prevkopgegverhandelingalgemeen)">
          <div class="ixsamenvatting">
            <xsl:apply-templates select="$samenvatting" mode="samenvatting"/>
          </div>
        </xsl:if>
        
        <div class="ixauteurgeg">
          <xsl:value-of select="$prev" />
        </div>
      </td>
    </tr>
  </xsl:template>

  <xsl:template match="div[@class='samenvatting']" mode="samenvatting">
    <xsl:apply-templates mode="samenvatting"/>
  </xsl:template>

  <!--<xsl:template match="span[@class='ablok' or @class='cursief']" mode="samenvatting">
    <xsl:apply-templates mode="samenvatting"/>
  </xsl:template>-->
  
  <!--<xsl:template match="span[not(@class='ablok') or not(@class='cursief')]" mode="samenvatting"/>-->
  <xsl:template match="span[not(@class='ablok') and not(@class='cursief')]" mode="samenvatting"/>
  
  <xsl:template match="@* | node()"  mode="samenvatting">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"  mode="samenvatting" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>
