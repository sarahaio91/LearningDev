<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="CAA | ABSTRACT | ESTREMI"/>


  <xsl:template match="/">
    <document>
      <xsl:apply-templates select="CIPER/LEGGI-SPEC"/>
    </document>
  </xsl:template>

  <xsl:template match="LEGGI-SPEC | COMUNE | DESCRIZIONE | COMMENTO | TESTO-COMM">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="ARTICOLO">
    <xsl:apply-templates select="*[name() != 'RIF']"/>
    <xsl:apply-templates select="RIF"/>
  </xsl:template>

  <xsl:template match="PARA">
    <xsl:choose>
      <xsl:when test="parent::DESCRIZIONE and ../parent::ARTICOLO">
        <h3>
          <bold>
            <xsl:text>Art. </xsl:text>
            <xsl:value-of select="../../@testo"/>
            <xsl:text> </xsl:text>
            <xsl:apply-templates/>
          </bold>
        </h3>
      </xsl:when>
      <xsl:when test="parent::DESCRIZIONE and ../parent::COMMENTO">
        <h6>
          <bold>
            <xsl:value-of select="../../@testo"/>
            <xsl:apply-templates/>
          </bold>
        </h6>
      </xsl:when>
      <xsl:otherwise>
        <p>
          <xsl:apply-templates/>
        </p>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="TIPOG">
    <italic>
      <xsl:apply-templates/>
    </italic>
  </xsl:template>

  <xsl:template match="POSTILLA">
    <div class="POSTILLA">
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match="RICH-LEGGE-ART">

  </xsl:template>


  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>
